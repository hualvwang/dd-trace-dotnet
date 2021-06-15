# .NET 5 only, as this is for running .NET Framework (IIS) tests primarily
ARG DOTNETSDK_VERSION
FROM mcr.microsoft.com/dotnet/sdk:$DOTNETSDK_VERSION-windowsservercore-ltsc2019

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Copy the build project in and build it
WORKDIR /project
COPY . /build
RUN dotnet build c:\\build
