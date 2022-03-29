﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;

namespace Datadog.Trace.Tagging
{
    partial class SqlTags
    {
        private static readonly byte[] SpanKindBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("span.kind");
        private static readonly byte[] DbTypeBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("db.type");
        private static readonly byte[] InstrumentationNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("component");
        private static readonly byte[] DbNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("db.name");
        private static readonly byte[] DbUserBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("db.user");
        private static readonly byte[] OutHostBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("out.host");

        public override string? GetTag(string key)
        {
            return key switch
            {
                "span.kind" => SpanKind,
                "db.type" => DbType,
                "component" => InstrumentationName,
                "db.name" => DbName,
                "db.user" => DbUser,
                "out.host" => OutHost,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "db.type": 
                    DbType = value;
                    break;
                case "component": 
                    InstrumentationName = value;
                    break;
                case "db.name": 
                    DbName = value;
                    break;
                case "db.user": 
                    DbUser = value;
                    break;
                case "out.host": 
                    OutHost = value;
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

            if (DbType != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, DbTypeBytes, DbType, tagProcessors);
            }

            if (InstrumentationName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, InstrumentationNameBytes, InstrumentationName, tagProcessors);
            }

            if (DbName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, DbNameBytes, DbName, tagProcessors);
            }

            if (DbUser != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, DbUserBytes, DbUser, tagProcessors);
            }

            if (OutHost != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, OutHostBytes, OutHost, tagProcessors);
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

            if (DbType != null)
            {
                sb.Append("db.type (tag):")
                  .Append(DbType)
                  .Append(',');
            }

            if (InstrumentationName != null)
            {
                sb.Append("component (tag):")
                  .Append(InstrumentationName)
                  .Append(',');
            }

            if (DbName != null)
            {
                sb.Append("db.name (tag):")
                  .Append(DbName)
                  .Append(',');
            }

            if (DbUser != null)
            {
                sb.Append("db.user (tag):")
                  .Append(DbUser)
                  .Append(',');
            }

            if (OutHost != null)
            {
                sb.Append("out.host (tag):")
                  .Append(OutHost)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
