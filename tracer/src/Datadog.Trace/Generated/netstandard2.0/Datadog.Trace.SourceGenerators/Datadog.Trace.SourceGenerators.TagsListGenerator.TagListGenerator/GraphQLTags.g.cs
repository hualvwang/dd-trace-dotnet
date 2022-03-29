﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.GraphQL
{
    partial class GraphQLTags
    {
        private static readonly byte[] SpanKindBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("span.kind");
        private static readonly byte[] InstrumentationNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("component");
        private static readonly byte[] SourceBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("graphql.source");
        private static readonly byte[] OperationNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("graphql.operation.name");
        private static readonly byte[] OperationTypeBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("graphql.operation.type");

        public override string? GetTag(string key)
        {
            return key switch
            {
                "span.kind" => SpanKind,
                "component" => InstrumentationName,
                "graphql.source" => Source,
                "graphql.operation.name" => OperationName,
                "graphql.operation.type" => OperationType,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "graphql.source": 
                    Source = value;
                    break;
                case "graphql.operation.name": 
                    OperationName = value;
                    break;
                case "graphql.operation.type": 
                    OperationType = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected override int WriteAdditionalTags(ref byte[] bytes, ref int offset, ITagProcessor[] tagProcessors)
        {
            var count = 0;
            if (SpanKind != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, SpanKindBytes, SpanKind, tagProcessors);
            }

            if (InstrumentationName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, InstrumentationNameBytes, InstrumentationName, tagProcessors);
            }

            if (Source != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, SourceBytes, Source, tagProcessors);
            }

            if (OperationName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, OperationNameBytes, OperationName, tagProcessors);
            }

            if (OperationType != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, OperationTypeBytes, OperationType, tagProcessors);
            }

            return count + base.WriteAdditionalTags(ref bytes, ref offset, tagProcessors);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (SpanKind != null)
            {
                sb.Append("span.kind (tag):")
                  .Append(SpanKind)
                  .Append(',');
            }

            if (InstrumentationName != null)
            {
                sb.Append("component (tag):")
                  .Append(InstrumentationName)
                  .Append(',');
            }

            if (Source != null)
            {
                sb.Append("graphql.source (tag):")
                  .Append(Source)
                  .Append(',');
            }

            if (OperationName != null)
            {
                sb.Append("graphql.operation.name (tag):")
                  .Append(OperationName)
                  .Append(',');
            }

            if (OperationType != null)
            {
                sb.Append("graphql.operation.type (tag):")
                  .Append(OperationType)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
