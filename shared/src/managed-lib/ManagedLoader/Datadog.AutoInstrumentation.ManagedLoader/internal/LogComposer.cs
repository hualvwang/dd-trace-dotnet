﻿// <auto-generated />
// This .CS file is automatically generated. If you modify its contents, your changes will be overwritten.
// Modify the respective T4 templates if changes are required.

// <auto-generated />
// ----------- ----------- ----------- ----------- -----------
// The source code below is included via a T4 template.
// The template calling must specify the value of the <c>NamespacesAndMonikersOfLogsToCompose</c> meta-variable.
// ----------- ----------- ----------- ----------- -----------

using System;
using System.Collections.Generic;

namespace Datadog.Logging.Composition
{
    /// <summary>
    /// Collects data from many Log-sources and sends it to the specified Log Sink.
    /// This class has been generated using a T4 template. It covers the following logging components:
    ///   1) Logger type:               "Datadog.AutoInstrumentation.ManagedLoader.Log"
    ///      Logging component moniker: "ManagedLoader"
    ///
    /// TOTAL: 1 loggers.
    /// </summary>
    internal static class LogComposer
    {
        private const string IsDebugLoggingEnabledEnvVarName = "DD_TRACE_DEBUG";

        private static bool s_isDebugLoggingEnabled = true;

        public static bool IsDebugLoggingEnabled
        {
            get
            {
                return s_isDebugLoggingEnabled;
            }

            set
            {
                s_isDebugLoggingEnabled = value;
                {
                    global::Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.DebugLoggingEnabled(s_isDebugLoggingEnabled);
                }
            }
        }

        public static void SetDebugLoggingEnabledBasedOnEnvironment()
        {
            bool envSetting = GetDebugLoggingEnabledEnvironmentSetting();
            IsDebugLoggingEnabled = envSetting;
        }

        public static void RedirectLogs(ILogSink logSink, out IReadOnlyDictionary<Type, LogSourceNameCompositionLogSink> redirectionLogSinks)
        {
            var redirectionSinks = new Dictionary<Type, LogSourceNameCompositionLogSink>(capacity: 2);

            {
                Type loggerType = typeof(global::Datadog.AutoInstrumentation.ManagedLoader.Log);
                const string logComponentGroupMoniker = "ManagedLoader";

                if (logSink == null)
                {
                    redirectionSinks[loggerType] = null;
                    global::Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.EventHandlers.Error(null);
                    global::Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.EventHandlers.Info(null);
                    global::Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.EventHandlers.Debug(null);
                }
                else
                {
                    var redirectionLogSink = new LogSourceNameCompositionLogSink(logComponentGroupMoniker, logSink);
                    var logToSinkAdapter = new LogEventHandlersToLogSinkAdapter(redirectionLogSink);
                    redirectionSinks[loggerType] = redirectionLogSink;
                    global::Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.EventHandlers.Error(logToSinkAdapter.Error);
                    global::Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.EventHandlers.Info(logToSinkAdapter.Info);
                    global::Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.EventHandlers.Debug(logToSinkAdapter.Debug);
                }

                Datadog.AutoInstrumentation.ManagedLoader.Log.Configure.DebugLoggingEnabled(IsDebugLoggingEnabled);
            }

            redirectionLogSinks = redirectionSinks;
        }

        private static bool GetDebugLoggingEnabledEnvironmentSetting()
        {
            // Unless the debug log is explicitly disabled, we assume that it is enabled.
            try
            {
                string IsDebugLoggingEnabledEnvVarValue = ReadEnvironmentVariable(IsDebugLoggingEnabledEnvVarName);

                if (IsDebugLoggingEnabledEnvVarValue != null)
                {
                    if (IsDebugLoggingEnabledEnvVarValue.Equals("false", System.StringComparison.OrdinalIgnoreCase)
                            || IsDebugLoggingEnabledEnvVarValue.Equals("no", System.StringComparison.OrdinalIgnoreCase)
                            || IsDebugLoggingEnabledEnvVarValue.Equals("n", System.StringComparison.OrdinalIgnoreCase)
                            || IsDebugLoggingEnabledEnvVarValue.Equals("f", System.StringComparison.OrdinalIgnoreCase)
                            || IsDebugLoggingEnabledEnvVarValue.Equals("0", System.StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }
            catch
            { }
            
            return true;
        }

        private static string ReadEnvironmentVariable(string envVarName)
        {
            try
            {
                return Environment.GetEnvironmentVariable(envVarName);
            }
            catch
            {
                return null;
            }
        }
    }
}
