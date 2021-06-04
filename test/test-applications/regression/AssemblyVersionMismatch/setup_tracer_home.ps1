param (
    [string]$tracer_version
)

$ProgressPreference = 'SilentlyContinue'
$ErrorActionPreference = 'Stop'

$dd_profiler_guid = "{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
$dd_tracer_home = ""
$dd_tracer_profiler_32 = ""
$dd_tracer_profiler_64 = ""

if ($tracer_version -eq "") {
    # Get the latest release tag from the github releases list
    Write-Output "Getting latest release version..."
    $tracer_version = (Invoke-WebRequest https://api.github.com/repos/datadog/dd-trace-dotnet/releases | ConvertFrom-Json)[0].tag_name.SubString(1)
}

Write-Output "Downloading tracer v$tracer_version..."

# Download the binary file depending of the current operating system and extract the content to the "tracer-home" folder
if ($env:os -eq "Windows_NT")
{
    $dd_tracer_home = "$(Get-Location)/tracer-home-$tracer_version"
    $dd_tracer_profiler_32 = "$dd_tracer_home/win-x86/Datadog.Trace.ClrProfiler.Native.dll"
    $dd_tracer_profiler_64 = "$dd_tracer_home/win-x64/Datadog.Trace.ClrProfiler.Native.dll"
    $url = "https://github.com/DataDog/dd-trace-dotnet/releases/download/v$($tracer_version)/windows-tracer-home.zip"
    $filename = "windows-tracer-home-$tracer_version.zip"
    Invoke-WebRequest -Uri "$url" -OutFile "$filename"

    Write-Output "Extracting file $filename..."
    Expand-Archive $filename -DestinationPath $dd_tracer_home
    Remove-Item $filename
}
else
{
    # File version is the same as the release version without the prerelease suffix.
    $file_version = $tracer_version.replace("-prerelease", "")
    $dd_tracer_home = "$(Get-Location)/tracer-home-$file_version"
    $dd_tracer_profiler_64 = "$dd_tracer_home/Datadog.Trace.ClrProfiler.Native.so"
    $url = "https://github.com/DataDog/dd-trace-dotnet/releases/download/v$($tracer_version)/datadog-dotnet-apm-$($file_version).tar.gz"
    $filename = "linux-tracer-home-$file_version.tar.gz"
    Invoke-WebRequest -Uri "$url" -OutFile "$filename"

    Write-Output "Extracting file $filename..."
    New-Item -Path "$dd_tracer_home" -ItemType Directory -Force
    tar -xvzf "$filename" -C "$dd_tracer_home"
    Remove-Item "$filename"

    # Ensure the native tracer library can write logs
    sudo mkdir -p /var/log/datadog/dotnet
    sudo chmod -R 777 /var/log/datadog/dotnet
}

# Set all environment variables to attach the profiler to the following pipeline steps
Write-Output "Setting environment variables..."

$Env:DD_ENV = "regression_tests"
$Env:DD_DOTNET_TRACER_HOME = "$dd_tracer_home"
$Env:DD_INTEGRATIONS = "$dd_tracer_home/integrations.json"

$Env:CORECLR_ENABLE_PROFILING = "1"
$Env:CORECLR_PROFILER = "$dd_profiler_guid"
$Env:CORECLR_PROFILER_PATH_32 = "$dd_tracer_profiler_32"
$Env:CORECLR_PROFILER_PATH_64 = "$dd_tracer_profiler_64"

$Env:COR_ENABLE_PROFILING = "1"
$Env:COR_PROFILER = "$dd_profiler_guid"
$Env:COR_PROFILER_PATH_32 = "$dd_tracer_profiler_32"
$Env:COR_PROFILER_PATH_64 = "$dd_tracer_profiler_64"

Write-Output "Done."
