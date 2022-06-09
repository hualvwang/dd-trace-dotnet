// <copyright file="RemoteSettings.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Datadog.Trace.Logging;
using Datadog.Trace.Vendors.Newtonsoft.Json;

namespace Datadog.Trace.Configuration
{
    internal class RemoteSettings : RemoteSettingsValues
    {
        private static readonly IDatadogLogger Logger = DatadogLogging.GetLoggerFor<RemoteSettings>();

        private readonly string _consulUrl;

        private readonly TimeSpan _consulUpdateInterval;

        private readonly HttpClient _httpClient;

        private RemoteSettings()
        {
            _consulUrl = Environment.GetEnvironmentVariable("DD_CONSUL_URL");
            var updateInterval = Environment.GetEnvironmentVariable("DD_CONSUL_UPDATE_INTERVAL");
            _consulUpdateInterval = updateInterval != null
                                        ? TimeSpan.FromSeconds(double.Parse(updateInterval))
                                        : TimeSpan.FromSeconds(60);
            if (string.IsNullOrEmpty(_consulUrl))
            {
                return;
            }

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_consulUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
            Task.Run(
                async () =>
                {
                    while (true)
                    {
                        try
                        {
                            await UpdateSettings().ConfigureAwait(false);
                            await Task.Delay(_consulUpdateInterval).ConfigureAwait(false);
                        }
                        catch
                        {
                            // ignore errors
                        }
                    }
                });
        }

        public static RemoteSettings Instance { get; } = new RemoteSettings();

        private async Task InitSettings()
        {
            var serviceName = Tracer.Instance.Settings.ServiceName;
            try
            {
                var response = await _httpClient.GetAsync($"/v1/kv/datadog/{serviceName}").ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    var httpResponseMessage = await _httpClient.PutAsync($"/v1/kv/datadog/{serviceName}", new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                    if (!httpResponseMessage.IsSuccessStatusCode)
                    {
                        Logger.Error($"Failed to initialize remote settings for service {serviceName}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task UpdateSettings()
        {
            var serviceName = Tracer.Instance.Settings.ServiceName;
            try
            {
                var response = await _httpClient.GetAsync($"/v1/kv/datadog/{serviceName}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var settings = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var settingsJson = JsonConvert.DeserializeObject<RemoteSettingsConsulResponse[]>(settings);
                    if (settingsJson != null)
                    {
                        var base64Str = settingsJson[0].Value;
                        var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));
                        var remoteSettings = JsonConvert.DeserializeObject<RemoteSettingsValues>(json);
                        TraceEnabled = remoteSettings.TraceEnabled;
                        StatsdEnabled = remoteSettings.StatsdEnabled;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Warning("Failed to update settings from Consul, {e}", e);
            }
        }
    }
}
