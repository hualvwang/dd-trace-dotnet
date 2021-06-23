Param (
    [Parameter(Mandatory = $true)]
    [String] $tracer_home
)

$ProgressPreference = 'SilentlyContinue'
$ErrorActionPreference = 'Stop'

$tracer_guid = '{846F5F1C-F9AE-4B07-969E-05C26BC060D8}'
$tracer_path_32 = ''
$tracer_path_64 = ''
$tracer_home = [System.IO.Path]::Combine((Get-Location), $tracer_home)
$logs_path = [System.IO.Path]::Combine($tracer_home, 'logs');

if ($env:os -eq 'Windows_NT') {
    Write-Output 'Setting environment variables for Windows'
    $tracer_path_32 = [System.IO.Path]::Combine($tracer_home, 'win-x86\Datadog.Trace.ClrProfiler.Native.dll');
    $tracer_path_64 = [System.IO.Path]::Combine($tracer_home, 'win-x64\Datadog.Trace.ClrProfiler.Native.dll');
}
else {
    Write-Verbose 'Setting environment variables for Linux'
    $tracer_path_64 = [System.IO.Path]::Combine($tracer_home, 'Datadog.Trace.ClrProfiler.Native.so');
}

$null = New-Item -Path $logs_path -ItemType Directory -Force

function Set-EnvironmentVariable {
    param([String] $name, [String] $value)

    Write-Verbose "$name=$($value.Replace($tracer_home, '$tracer_home'))"
    Set-Item -Path "Env:$name" -Value $value
}

# Set the environment variables to attach the tracer
Set-EnvironmentVariable 'DD_DOTNET_TRACER_HOME' $tracer_home
Set-EnvironmentVariable 'DD_INTEGRATIONS' ([System.IO.Path]::Combine($tracer_home, 'integrations.json'))
Set-EnvironmentVariable 'DD_TRACE_LOG_DIRECTORY' $logs_path
Set-EnvironmentVariable 'DD_PROFILER_EXCLUDE_PROCESSES' 'dotnet.exe;devenv.exe;Microsoft.ServiceHub.Controller.exe;ServiceHub.Host.CLR.exe;ServiceHub.TestWindowStoreHost.exe;ServiceHub.DataWarehouseHost.exe;sqlservr.exe;VBCSCompiler.exe;iisexpresstray.exe;msvsmon.exe;PerfWatson2.exe;ServiceHub.IdentityHost.exe;ServiceHub.VSDetouredHost.exe;ServiceHub.SettingsHost.exe;ServiceHub.Host.CLR.x86.exe;vstest.console.exe;ServiceHub.RoslynCodeAnalysisService32.exe;testhost.x86.exe;MSBuild.exe;ServiceHub.ThreadedWaitDialog.exe'

Set-EnvironmentVariable 'CORECLR_ENABLE_PROFILING' '1'
Set-EnvironmentVariable 'CORECLR_PROFILER' $tracer_guid
Set-EnvironmentVariable 'CORECLR_PROFILER_PATH_32' $tracer_path_32
Set-EnvironmentVariable 'CORECLR_PROFILER_PATH_64' $tracer_path_64

Set-EnvironmentVariable 'COR_ENABLE_PROFILING' '1'
Set-EnvironmentVariable 'COR_PROFILER' $tracer_guid
Set-EnvironmentVariable 'COR_PROFILER_PATH_32' $tracer_path_32
Set-EnvironmentVariable 'COR_PROFILER_PATH_64' $tracer_path_64