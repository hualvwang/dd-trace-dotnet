# Assembly version mismatch tests

These scripts will test an application which loads different versions of the .NET Tracer from an installer package and a nuget package. Each tests uses two version of the tracer, "older" and "newer".

"Pre-merge" refers to a tracer using the original way the solution was organized, with separate assemblies for:
- Datadog.Trace
- Datadog.Trace.ClrProfiler.Managed
- Datadog.Trace.ClrProfiler.Managed.Core
- Datadog.Trace.AspNet

"Post-merge" refers to a tracer using the single `Datadog.Trace` assembly which includes all the code from the multiple assemblies listed above.

## Test matrix

Runtime        | MSI              | NuGet            | Test Result
---------------|------------------|------------------|-------
.NET Framework | older pre-merge  | newer            |
.NET Framework | older post-merge | newer            |
.NET Framework | newer            | older pre-merge  |
.NET Framework | newer            | older post-merge |
.NET Framework | same             | same             |
.NET Core      | older pre-merge  | newer            |
.NET Core      | older post-merge | newer            |
.NET Core      | newer            | older pre-merge  |
.NET Core      | newer            | older post-merge |
.NET Core      | same             | same             |

## Steps to test each scenario

Older pre-merge MSI with newer NuGet:
- msi: download and install msi `{VERSION}` from github.com
- nuget: update source code with `{VERSION+1}`, build nuget

Older post-merge MSI with newer NuGet:
- msi: update source code with `{VERSION}`, build and install msi
- nuget: update source code with `{VERSION+1}`, build nuget

Newer MSI with older pre-merge Nuget:
- msi: update source code with `{VERSION+1}`, build and install msi
- nuget: reference nuget `{VERSION}` from nuget.org

Newer MSI with older post-merge Nuget:
- msi: update source code with `{VERSION+1}`, build and install msi
- nuget: update source code with `{VERSION}`, build nuget
