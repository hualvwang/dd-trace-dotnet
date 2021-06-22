// <copyright file="IisFixture.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Diagnostics;
using System.IO;
using Datadog.Core.Tools;
using Datadog.Trace.TestHelpers;

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{
    public sealed class IisFixture : IDisposable
    {
        private (Process Process, string ConfigFile) _iisExpress;

        public MockTracerAgent Agent { get; private set; }

        public int HttpPort { get; private set; }

        public void TryStartIis(TestHelper helper, bool classicMode)
        {
            lock (this)
            {
                if (_iisExpress.Process == null)
                {
                    var initialAgentPort = TcpPortProvider.GetOpenPort();
                    Agent = new MockTracerAgent(initialAgentPort);

                    HttpPort = TcpPortProvider.GetOpenPort();

                    _iisExpress = helper.StartIISExpress(Agent.Port, HttpPort, classicMode);
                }
            }
        }

        public void Dispose()
        {
            Agent?.Dispose();

            lock (this)
            {
                if (_iisExpress.Process != null)
                {
                    try
                    {
                        if (!_iisExpress.Process.HasExited)
                        {
                            // sending "Q" to standard input does not work because
                            // iisexpress is scanning console key press, so just kill it.
                            // maybe try this in the future:
                            // https://github.com/roryprimrose/Headless/blob/master/Headless.IntegrationTests/IisExpress.cs
                            _iisExpress.Process.Kill();
                            _iisExpress.Process.WaitForExit(8000);
                        }
                    }
                    catch
                    {
                        // in some circumstances the HasExited property throws, this means the process probably hasn't even started correctly
                    }

                    _iisExpress.Process.Dispose();

                    try
                    {
                        File.Delete(_iisExpress.ConfigFile);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}