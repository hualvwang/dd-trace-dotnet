// <copyright file="TestAppFact.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using Xunit;
using Xunit.Sdk;

namespace Datadog.Profiler.SmokeTests
{
    [XunitTestCaseDiscoverer("Datadog.Profiler.SmokeTests.TestAppFrameworkDiscover", "Datadog.Profiler.IntegrationTests")]
    internal class TestAppFact : FactAttribute
    {
        public TestAppFact(string appAssembly)
        {
            AppAssembly = appAssembly;
            AppName = appAssembly;
        }

        public string AppAssembly { get; }

        public string AppName { get; set; }
    }
}
