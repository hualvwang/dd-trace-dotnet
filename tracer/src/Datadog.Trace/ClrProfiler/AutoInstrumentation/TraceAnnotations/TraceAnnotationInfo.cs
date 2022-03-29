// <copyright file="TraceAnnotationInfo.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

#nullable enable

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.TraceAnnotations
{
    internal readonly struct TraceAnnotationInfo
    {
        private const string DefaultOperationName = "trace.annotation";

        public static readonly TraceAnnotationInfo Default = new TraceAnnotationInfo(resourceName: "unknown");

        public readonly string OperationName;

        public readonly string ResourceName;

        public TraceAnnotationInfo(string resourceName, string operationName = DefaultOperationName)
        {
            OperationName = operationName;
            ResourceName = resourceName;
        }
    }
}
