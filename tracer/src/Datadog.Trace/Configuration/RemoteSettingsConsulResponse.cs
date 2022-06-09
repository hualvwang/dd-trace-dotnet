// <copyright file="RemoteSettingsConsulResponse.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

namespace Datadog.Trace.Configuration
{
    internal class RemoteSettingsConsulResponse
    {
        public int CreateIndex { get; set; }

        public int ModifyIndex { get; set; }

        public int LockIndex { get; set; }

        public string Key { get; set; }

        public int Flags { get; set; }

        public string Value { get; set; }

        public string Session { get; set; }
    }
}
