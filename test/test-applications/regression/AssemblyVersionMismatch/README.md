# Assembly version mismatch tests

These scripts will test an application which loads different versions of the .NET Tracer: one from the tracer home directory (like from an installer package) and another directly referenced by the app (like a NuGet package).

A "Pre-merge" tracer version contains separate assemblies for:
- Datadog.Trace
- Datadog.Trace.ClrProfiler.Managed
- Datadog.Trace.ClrProfiler.Managed.Core
- Datadog.Trace.AspNet

A "Post-merge" version refers to a tracer using the single `Datadog.Trace` assembly which includes all the code from the assemblies listed above.

## Test matrix

Tracer Home  | NuGet
-------------|-------------
post-merge   | (same version)
pre-merge    | post-merge
post-merge   | pre-merge
post-merge   | post-merge+1
post-merge+1 | post-merge
post-merge   | (same version)
pre-merge    | post-merge
post-merge   | pre-merge
post-merge   | post-merge+1
post-merge+1 | post-merge

## Steps to test each scenario

Determine versions:

Name         | Version
-------------|--------
Pre-merge    | 1.27.0
Post-merge   | _current version_
Post-merge+1 | _current version_ + 0.1

Tracer home:
- download zip file `1.27.0` from github.com
-

Pre-merge tracer home with newer `Datadog.Trce.dll`:
- tracer home: download zip file `1.27.0` from github.com
- nuget: update source code with new version, build nuget

Post-merge tracer home with newer `Datadog.Trce.dll`:
- tracer home: update source code with `{VERSION}`, build and install msi
- nuget: update source code with new version, build nuget

Newer MSI with older pre-merge Nuget:
- msi: update source code with `{VERSION+1}`, build and install msi
- nuget: reference nuget `{VERSION}` from nuget.org

Newer MSI with older post-merge Nuget:
- msi: update source code with `{VERSION+1}`, build and install msi
- nuget: update source code with `{VERSION}`, build nuget
