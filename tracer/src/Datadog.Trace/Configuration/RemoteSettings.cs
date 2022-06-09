// <copyright file="RemoteSettings.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Datadog.Trace.Agent;
using Datadog.Trace.Agent.Transports;
using Datadog.Trace.Logging;
using Datadog.Trace.Vendors.Newtonsoft.Json;

namespace Datadog.Trace.Configuration
{
    internal class RemoteSettings : RemoteSettingsValues
    {
        private static readonly IDatadogLogger Logger = DatadogLogging.GetLoggerFor<RemoteSettings>();

        private readonly string _consulUrl;

        private readonly TimeSpan _consulUpdateInterval;

        private readonly IApiRequestFactory _apiRequestFactory;

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

#if NETCOREAPP
            _apiRequestFactory = new HttpClientRequestFactory(new Uri(_consulUrl), Array.Empty<KeyValuePair<string, string>>());
#else
            _apiRequestFactory = new ApiWebRequestFactory(new Uri(_consulUrl), Array.Empty<KeyValuePair<string, string>>());
#endif
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
                var response = await _apiRequestFactory.Create(_apiRequestFactory.GetEndpoint($"/v1/kv/datadog/{serviceName}")).GetAsync().ConfigureAwait(false);
                if (response.StatusCode >= 400)
                {
                    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
                    var httpResponseMessage = await _apiRequestFactory.Create(_apiRequestFactory.GetEndpoint($"/v1/kv/datadog/{serviceName}")).PutAsync(new ArraySegment<byte>(bytes), "application/json").ConfigureAwait(false);
                    if (httpResponseMessage.StatusCode >= 400)
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
                var response = await _apiRequestFactory.Create(_apiRequestFactory.GetEndpoint($"/v1/kv/datadog/{serviceName}")).GetAsync().ConfigureAwait(false);
                if (response.StatusCode < 400)
                {
                    var settings = await response.ReadAsStringAsync().ConfigureAwait(false);
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
