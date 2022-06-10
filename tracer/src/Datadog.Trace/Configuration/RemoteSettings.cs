// <copyright file="RemoteSettings.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

        private readonly string _consulResource;

        private readonly IApiRequestFactory _apiRequestFactory;

        private RemoteSettings()
        {
            var consulUrl = Environment.GetEnvironmentVariable("DD_CONSUL_URL");
            var consulResourcePath = Environment.GetEnvironmentVariable("DD_CONSUL_RESOURCE_PATH");
            var updateInterval = Environment.GetEnvironmentVariable("DD_CONSUL_UPDATE_INTERVAL") ?? "/v1/kv/datadog/";
            var consulUpdateInterval = updateInterval != null
                                            ? TimeSpan.FromSeconds(double.Parse(updateInterval))
                                            : TimeSpan.FromSeconds(60);
            if (string.IsNullOrEmpty(consulUrl))
            {
                return;
            }

            _consulResource = $"{consulResourcePath}{Tracer.Instance.Settings.ServiceName}";

#if NETCOREAPP
            _apiRequestFactory = new HttpClientRequestFactory(new Uri(consulUrl), Array.Empty<KeyValuePair<string, string>>());
#else
            _apiRequestFactory = new ApiWebRequestFactory(new Uri(_consulUrl), Array.Empty<KeyValuePair<string, string>>());
#endif
            Task.Run(
                async () =>
                {
                    await InitSettings().ConfigureAwait(false);
                    while (true)
                    {
                        try
                        {
                            await UpdateSettings().ConfigureAwait(false);
                            await Task.Delay(consulUpdateInterval).ConfigureAwait(false);
                        }
                        catch (ThreadAbortException)
                        {
                            throw;
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, "Error while updating settings from consul");
                        }
                    }
                });
        }

        public static RemoteSettings Instance { get; } = new RemoteSettings();

        private async Task InitSettings()
        {
            try
            {
                var response = await _apiRequestFactory.Create(_apiRequestFactory.GetEndpoint(_consulResource)).GetAsync().ConfigureAwait(false);
                if (response.StatusCode >= 400)
                {
                    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
                    var httpResponseMessage = await _apiRequestFactory.Create(_apiRequestFactory.GetEndpoint(_consulResource)).PutAsync(new ArraySegment<byte>(bytes), "application/json").ConfigureAwait(false);
                    if (httpResponseMessage.StatusCode >= 400)
                    {
                        Logger.Error("Failed to initialize remote settings for service");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed to initialize remote settings, {e}", e);
            }
        }

        private async Task UpdateSettings()
        {
            try
            {
                var response = await _apiRequestFactory.Create(_apiRequestFactory.GetEndpoint(_consulResource)).GetAsync().ConfigureAwait(false);
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
                else
                {
                    Logger.Warning("Failed to update remote settings, {response.StatusCode}", response.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {
                Logger.Warning("Failed to update settings from Consul, {e}", e);
            }
        }
    }
}
