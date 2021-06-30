// <copyright file="IFunctionInstance.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.Azure.Functions
{
    /// <summary>
    /// For duck typing
    /// </summary>
    public interface IFunctionInstance
    {
        /// <summary>
        /// Gets Function unique id
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets Function invoker object
        /// </summary>
        IFunctionInvoker Invoker { get; }

        /// <summary>
        /// Gets Function description object
        /// </summary>
        IFunctionDescriptor FunctionDescriptor { get; }
    }
}
