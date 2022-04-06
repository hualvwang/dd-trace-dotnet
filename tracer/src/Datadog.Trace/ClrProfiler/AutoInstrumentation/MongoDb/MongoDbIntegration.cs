// <copyright file="MongoDbIntegration.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Datadog.Trace.Configuration;
using Datadog.Trace.DuckTyping;
using Datadog.Trace.Logging;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.MongoDb
{
    /// <summary>
    /// Tracing integration for MongoDB.Driver.Core.
    /// </summary>
    internal static class MongoDbIntegration
    {
        internal const string IntegrationName = nameof(Configuration.IntegrationId.MongoDb);

        internal const string Major2 = "2";
        internal const string Major2Minor1 = "2.1";
        internal const string Major2Minor2 = "2.2"; // Synchronous methods added in 2.2
        internal const string MongoDbClientAssembly = "MongoDB.Driver.Core";

        private const string OperationName = "mongodb.query";
        private const string ServiceName = "mongodb";

        internal const IntegrationId IntegrationId = Configuration.IntegrationId.MongoDb;

        private static readonly IDatadogLogger Log = DatadogLogging.GetLoggerFor(typeof(MongoDbIntegration));

        internal static Scope CreateScope<TConnection>(object wireProtocol, TConnection connection)
            where TConnection : IConnection
        {
            var tracer = Tracer.Instance;

            if (!tracer.Settings.IsIntegrationEnabled(IntegrationId))
            {
                // integration disabled, don't create a scope, skip this trace
                return null;
            }

            if (GetActiveMongoDbScope(tracer) != null)
            {
                // There is already a parent MongoDb span (nested calls)
                return null;
            }

            string collectionName = null;
            string query = null;
            string resourceName = null;
            string databaseName = null;

            if (wireProtocol.TryDuckCast<IWireProtocolWithDatabaseNamespaceStruct>(out var protocolWithDatabaseNamespace))
            {
                databaseName = protocolWithDatabaseNamespace.DatabaseNamespace.DatabaseName;
            }

            if (wireProtocol.TryDuckCast<IWireProtocolWithCommandStruct>(out var protocolWithCommand)
             && protocolWithCommand.Command != null)
            {
                try
                {
                    // the name of the first element in the command BsonDocument will be the operation type (insert, delete, find, etc)
                    // and its value is the collection name
                    var firstElement = protocolWithCommand.Command.GetElement(0);
                    string operationName = firstElement.Name;

                    if (operationName == "isMaster" || operationName == "hello")
                    {
                        return null;
                    }

                    // resourceName = $"{operationName ?? "operation"} {databaseName ?? "database"}";
                    resourceName = JsonObfuscator.FromJson(protocolWithCommand.Command.ToString());
                    collectionName = firstElement.Value?.ToString();
                    query = protocolWithCommand.Command.ToString();
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Unable to access IWireProtocol.Command properties.");
                }
            }

            string host = null;
            string port = null;

            if (connection.EndPoint is IPEndPoint ipEndPoint)
            {
                host = ipEndPoint.Address.ToString();
                port = ipEndPoint.Port.ToString();
            }
            else if (connection.EndPoint is DnsEndPoint dnsEndPoint)
            {
                host = dnsEndPoint.Host;
                port = dnsEndPoint.Port.ToString();
            }

            string serviceName;

            if (tracer.Settings.DbClientSplitByInstance)
            {
                serviceName = databaseName ?? tracer.Settings.GetServiceName(tracer, ServiceName);
            }
            else
            {
                serviceName = tracer.Settings.GetServiceName(tracer, ServiceName);
            }

            Scope scope = null;

            try
            {
                var tags = new MongoDbTags();
                scope = tracer.StartActiveInternal(OperationName, serviceName: serviceName, tags: tags);
                var span = scope.Span;
                span.Type = SpanTypes.MongoDb;
                span.ResourceName = resourceName;
                tags.DbName = databaseName;
                tags.Query = query;
                tags.Collection = collectionName;
                tags.Host = host;
                tags.Port = port;

                tags.SetAnalyticsSampleRate(IntegrationId, tracer.Settings, enabledWithGlobalSetting: false);
                tracer.TracerManager.Telemetry.IntegrationGeneratedSpan(IntegrationId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating or populating scope.");
            }

            return scope;
        }

        private static Scope GetActiveMongoDbScope(Tracer tracer)
        {
            var scope = tracer.InternalActiveScope;

            var parent = scope?.Span;

            if (parent != null &&
                parent.Type == SpanTypes.MongoDb &&
                parent.GetTag(Tags.InstrumentationName) != null)
            {
                return scope;
            }

            return null;
        }

        private static class JsonObfuscator
        {
            [ThreadStatic]
            private static StringBuilder _stringBuilder;

            [ThreadStatic]
            private static StringBuilder _stringBuilderRes;

            public static string FromJson(string json)
            {
                if (json == null)
                {
                    return null;
                }

                _stringBuilder ??= new StringBuilder();
                _stringBuilderRes ??= new StringBuilder();
                _stringBuilder.Clear();

                for (var i = 0; i < json.Length; i++)
                {
                    var c = json[i];
                    if (c == '"')
                    {
                        i = AppendUntilStringEnd(i, json);
                        continue;
                    }

                    if (char.IsWhiteSpace(c))
                    {
                        continue;
                    }

                    _stringBuilder.Append(c);
                }

                var s = _stringBuilder.ToString();
                _stringBuilder.Clear();
                ParseAnonymousValue(s);
                s = _stringBuilderRes.ToString();
                _stringBuilderRes.Clear();
                return s;
            }

            private static int AppendUntilStringEnd(int startIdx, string json, char end = '"')
            {
                _stringBuilder.Append(json[startIdx]);
                for (var i = startIdx + 1; i < json.Length; i++)
                {
                    _stringBuilder.Append(json[i]);
                    if (json[i] == end)
                    {
                        return i;
                    }
                }

                return json.Length - 1;
            }

            private static List<string> Split(string json)
            {
                var splitArray = new List<string>();
                if (json.Length == 2)
                {
                    return splitArray;
                }

                var parseDepth = 0;
                _stringBuilder.Clear();
                for (var i = 1; i < json.Length - 1; i++)
                {
                    switch (json[i])
                    {
                        case '[':
                        case '{':
                            parseDepth++;
                            break;
                        case ']':
                        case '}':
                            parseDepth--;
                            break;
                        case '"':
                            i = AppendUntilStringEnd(i, json);
                            continue;
                        case ',':
                        case ':':
                            if (parseDepth == 0)
                            {
                                splitArray.Add(_stringBuilder.ToString());
                                _stringBuilder.Clear();
                                continue;
                            }

                            break;
                        case 'n' when json[i + 1] == 'e' && json[i + 2] == 'w': // newXXX
                        case 'O' when json[i + 1] == 'b' && json[i + 2] == 'j': // ObjectId
                        case 'I' when json[i + 1] == 'S' && json[i + 2] == 'O': // ISODate
                        case 'N' when json[i + 1] == 'u' && json[i + 2] == 'm': // NumberDecimal / NumberLong
                        case 'T' when json[i + 1] == 'i' && json[i + 2] == 'm': // Timestamp
                            i = AppendUntilStringEnd(i, json, ')');
                            continue;
                    }

                    _stringBuilder.Append(json[i]);
                }

                splitArray.Add(_stringBuilder.ToString());
                _stringBuilder.Clear();
                return splitArray;
            }

            private static Tuple<string, bool> ParseAnonymousValue(string json, string name = null)
            {
                if (json.Length == 0)
                {
                    return new Tuple<string, bool>(null, true);
                }

                if (json[0] == '{' && json[json.Length - 1] == '}')
                {
                    return ProcessObject(json, name);
                }

                if (json[0] == '[' && json[json.Length - 1] == ']')
                {
                    return ProcessArray(json, name);
                }

                return new Tuple<string, bool>("\"?\"", true);
            }

            private static Tuple<string, bool> ProcessObject(string json, string name)
            {
                var elems = Split(json);
                if (elems.Count % 2 != 0)
                {
                    return new Tuple<string, bool>(null, true);
                }

                if (name != null)
                {
                    _stringBuilderRes.Append(name).Append(":");
                }

                _stringBuilderRes.Append('{');
                for (var i = 0; i < elems.Count; i += 2)
                {
                    var tuple = ParseAnonymousValue(elems[i + 1], elems[i]);
                    if (tuple.Item2)
                    {
                        _stringBuilderRes.Append(elems[i]).Append(':').Append(tuple.Item1);
                    }

                    if (i != elems.Count - 2)
                    {
                        _stringBuilderRes.Append(',');
                    }
                }

                _stringBuilderRes.Append('}');
                return new Tuple<string, bool>(null, false);
            }

            private static Tuple<string, bool> ProcessArray(string json, string name)
            {
                var items = Split(json);
                if (name != null)
                {
                    _stringBuilderRes.Append(name).Append(":");
                }

                _stringBuilderRes.Append('[');
                for (var i = 0; i < items.Count; i++)
                {
                    var tuple = ParseAnonymousValue(items[i]);
                    _stringBuilderRes.Append(tuple.Item1);
                    if (i != items.Count - 1)
                    {
                        _stringBuilderRes.Append(',');
                    }
                }

                _stringBuilderRes.Append(']');
                return new Tuple<string, bool>(null, false);
            }
        }
    }
}
