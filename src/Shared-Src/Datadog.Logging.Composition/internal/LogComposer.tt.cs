// <auto-generated />
// ----------- ----------- ----------- ----------- -----------
// The source code below is included via a T4 template.
// The template calling must specify the value of the <c>NamespacesAndMonikersOfLogsToCompose</c> meta-variable.
// ----------- ----------- ----------- ----------- -----------
<#@ parameter type="System.Tuple<System.String, System.String>[]" name="NamespacesAndMonikersOfLogsToCompose" #>
<# Tuple<string, string>[] NamespacesAndMonikersToCompose = NamespacesAndMonikersOfLogsToCompose ?? new Tuple<string, string>[0]; #>

using System;

namespace Datadog.Logging.Composition
{
    /// <summary>
    /// Collects data from many Log-sources and sends it to the specified Log Sink.
    /// This class has been generated using a T4 template. It covers the following logging components:
<#
    int validLogNsCount = 0;
    for (int i = 0; i < NamespacesAndMonikersToCompose.Length; i++)
    {
        string logNamespace = NamespacesAndMonikersToCompose[i]?.Item1;
        string logComponentMoniker = NamespacesAndMonikersToCompose[i]?.Item2;
        if (String.IsNullOrWhiteSpace(logNamespace))
        {
            continue;
        }

        validLogNsCount++;
#>
    ///   <#= validLogNsCount #>) Logger type:               "<#= logNamespace #>.Log"
    ///      Logging component moniker: "<#= logComponentMoniker #>"
    ///
<#
    }
#>
    /// TOTAL: <#= validLogNsCount #> loggers.
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
<#
                for (int i = 0; i < NamespacesAndMonikersToCompose.Length; i++)
                {
                    string logNamespace = NamespacesAndMonikersToCompose[i]?.Item1;
                    if (String.IsNullOrWhiteSpace(logNamespace))
                    {
                        continue;
                    }
#>
                    global::<#= logNamespace #>.Log.Configure.DebugLoggingEnabled(s_isDebugLoggingEnabled);
<#
                }
#>
                }
            }
        }

        public static void SetDebugLoggingEnabledBasedOnEnvironment()
        {
            bool envSetting = GetDebugLoggingEnabledEnvironmentSetting();
            IsDebugLoggingEnabled = envSetting;
        }

        public static void RedirectLogs(ILogSink logSink)
        {
            {
<#
            for (int i = 0; i < NamespacesAndMonikersToCompose.Length; i++)
            {
                string logNamespace = NamespacesAndMonikersToCompose[i]?.Item1;
                string logComponentMoniker = NamespacesAndMonikersToCompose[i]?.Item2;
                if (String.IsNullOrWhiteSpace(logNamespace))
                {
                    continue;
                }
#>
                <#= logNamespace #>.Log.Configure.DebugLoggingEnabled(IsDebugLoggingEnabled);

                if (logSink == null)
                {
                    global::<#= logNamespace #>.Log.Configure.Error(null);
                    global::<#= logNamespace #>.Log.Configure.Info(null);
                    global::<#= logNamespace #>.Log.Configure.Debug(null);
                }
                else
                {
                    global::<#= logNamespace #>.Log.Configure.Error((component, msg, ex, data) => logSink.Error(StringPair.Create("<#= logComponentMoniker #>", component), msg, ex, data));
                    global::<#= logNamespace #>.Log.Configure.Info((component, msg, data) => logSink.Info(StringPair.Create("<#= logComponentMoniker #>", component), msg, data));
                    global::<#= logNamespace #>.Log.Configure.Debug((component, msg, data) => { if (IsDebugLoggingEnabled) { logSink.Debug(StringPair.Create("<#= logComponentMoniker #>", component), msg, data); } });
                }
<#
            }
#>
            }
        }

        private static bool GetDebugLoggingEnabledEnvironmentSetting()
        {
            // Unless the debug log is explicitly disabled, we assume that it is enabled.
            try
            {
                string IsDebugLoggingEnabledEnvVarValue = Environment.GetEnvironmentVariable(IsDebugLoggingEnabledEnvVarName);

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
    }
}