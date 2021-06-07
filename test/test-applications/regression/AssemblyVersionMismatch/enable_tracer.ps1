Param (
    [Parameter(Mandatory = $true)]
    [String]
    $tracer_home
)

$ProgressPreference = "SilentlyContinue"
$ErrorActionPreference = "Stop"

$tracer_guid = "{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
$tracer_path_32 = ""
$tracer_path_64 = ""
$logs_path = "$tracer_home/logs/"

if ($env:os -eq "Windows_NT") {
    $tracer_path_32 = "$tracer_home/win-x86/Datadog.Trace.ClrProfiler.Native.dll"
    $tracer_path_64 = "$tracer_home/win-x64/Datadog.Trace.ClrProfiler.Native.dll"
}
else
{
    $tracer_path_64 = "$tracer_home/Datadog.Trace.ClrProfiler.Native.so"
}

New-Item -Path $logs_path -ItemType Directory -Force

# Set the environment variables to attach the tracer
Write-Output "Setting environment variables..."

$Env:DD_ENV = "regression_tests"
$Env:DD_DOTNET_TRACER_HOME = $tracer_home
$Env:DD_INTEGRATIONS = "$tracer_home/integrations.json"
$Enf:DD_TRACE_LOG_DIRECTORY = $logs_path

$Env:CORECLR_ENABLE_PROFILING = "1"
$Env:CORECLR_PROFILER = $tracer_guid
$Env:CORECLR_PROFILER_PATH_32 = $tracer_path_32
$Env:CORECLR_PROFILER_PATH_64 = $tracer_path_64

$Env:COR_ENABLE_PROFILING = "1"
$Env:COR_PROFILER = $tracer_guid
$Env:COR_PROFILER_PATH_32 = $tracer_path_32
$Env:COR_PROFILER_PATH_64 = $tracer_path_64

Write-Output "Done."
