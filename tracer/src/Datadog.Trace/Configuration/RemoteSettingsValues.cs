// <copyright file="RemoteSettingsValues.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

namespace Datadog.Trace.Configuration
{
    internal class RemoteSettingsValues
    {
        public bool TraceEnabled { get; set; } = true;

        public bool StatsdEnabled { get; set; } = true;
    }
}
