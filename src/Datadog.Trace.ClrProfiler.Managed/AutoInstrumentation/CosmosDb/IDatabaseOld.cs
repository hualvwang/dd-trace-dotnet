// <copyright file="IDatabaseOld.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.CosmosDb
{
    /// <summary>
    /// Microsoft.Azure.Cosmos.Database for duct typing
    /// </summary>
    public interface IDatabaseOld
    {
        /// <summary>
        /// Gets the Id of the Cosmos database
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the parent Cosmos client instance related the database instance
        /// </summary>
        ICosmosContextClient ClientContext { get; }
    }
}
