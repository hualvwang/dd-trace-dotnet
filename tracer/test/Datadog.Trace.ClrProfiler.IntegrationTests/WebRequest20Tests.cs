// <copyright file="WebRequest20Tests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

#if NETFRAMEWORK
using System.Globalization;
using System.Linq;
using Datadog.Trace.ClrProfiler.IntegrationTests.Helpers;
using Datadog.Trace.Configuration;
using Datadog.Trace.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{
    public class WebRequest20Tests : TestHelper
    {
        public WebRequest20Tests(ITestOutputHelper output)
            : base("WebRequest.NetFramework20", output)
        {
            SetServiceVersion("1.0.0");
        }

        [SkippableFact]
        [Trait("Category", "EndToEnd")]
        [Trait("RunOnWindows", "True")]
        public void SubmitsTraces()
        {
            int expectedSpanCount = 45;
            const string expectedOperationName = "http.request";
            const string expectedServiceName = "Samples.WebRequest.NetFramework20-http-client";

            int httpPort = TcpPortProvider.GetOpenPort();
            Output.WriteLine($"Assigning port {httpPort} for the httpPort.");

            using var telemetry = this.ConfigureTelemetry();
            using (var agent = EnvironmentHelper.GetMockAgent())
            using (ProcessResult processResult = RunSampleAndWaitForExit(agent, arguments: $"Port={httpPort}"))
            {
                var spans = agent.WaitForSpans(expectedSpanCount, operationName: expectedOperationName);
                Assert.Equal(expectedSpanCount, spans.Count);

                foreach (var span in spans)
                {
                    Assert.Equal(expectedOperationName, span.Name);
                    Assert.Equal(expectedServiceName, span.Service);
                    Assert.Equal(SpanTypes.Http, span.Type);
                    Assert.Equal("WebRequest", span.Tags[Tags.InstrumentationName]);
                    Assert.False(span.Tags?.ContainsKey(Tags.Version), "External service span should not have service version tag.");
                }

                var firstSpan = spans.First();
                var traceId = StringUtil.GetHeader(processResult.StandardOutput, HttpHeaderNames.TraceId);
                var parentSpanId = StringUtil.GetHeader(processResult.StandardOutput, HttpHeaderNames.ParentId);

                Assert.Equal(firstSpan.TraceId.ToString(CultureInfo.InvariantCulture), traceId);
                Assert.Equal(firstSpan.SpanId.ToString(CultureInfo.InvariantCulture), parentSpanId);

                telemetry.AssertIntegrationEnabled(IntegrationId.WebRequest);
            }
        }

        [SkippableFact]
        [Trait("Category", "EndToEnd")]
        [Trait("RunOnWindows", "True")]
        public void TracingDisabled_DoesNotSubmitsTraces()
        {
            const string expectedOperationName = "http.request";

            int httpPort = TcpPortProvider.GetOpenPort();

            using var telemetry = this.ConfigureTelemetry();
            using (var agent = EnvironmentHelper.GetMockAgent())
            using (ProcessResult processResult = RunSampleAndWaitForExit(agent, arguments: $"TracingDisabled Port={httpPort}"))
            {
                var spans = agent.WaitForSpans(1, 3000, operationName: expectedOperationName);
                Assert.Equal(0, spans.Count);

                var traceId = StringUtil.GetHeader(processResult.StandardOutput, HttpHeaderNames.TraceId);
                var parentSpanId = StringUtil.GetHeader(processResult.StandardOutput, HttpHeaderNames.ParentId);
                var tracingEnabled = StringUtil.GetHeader(processResult.StandardOutput, HttpHeaderNames.TracingEnabled);

                Assert.Null(traceId);
                Assert.Null(parentSpanId);
                Assert.Equal("false", tracingEnabled);
                telemetry.AssertIntegrationDisabled(IntegrationId.WebRequest);
            }
        }
    }
}
#endif