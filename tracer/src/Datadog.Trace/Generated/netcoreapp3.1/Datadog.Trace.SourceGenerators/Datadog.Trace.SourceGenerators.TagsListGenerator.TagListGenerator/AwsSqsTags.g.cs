﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;

namespace Datadog.Trace.Tagging
{
    partial class AwsSqsTags
    {
        private static readonly byte[] QueueNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aws.queue.name");
        private static readonly byte[] QueueUrlBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aws.queue.url");
        private static readonly byte[] SpanKindBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("span.kind");

        public override string? GetTag(string key)
        {
            return key switch
            {
                "aws.queue.name" => QueueName,
                "aws.queue.url" => QueueUrl,
                "span.kind" => SpanKind,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "aws.queue.name": 
                    QueueName = value;
                    break;
                case "aws.queue.url": 
                    QueueUrl = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected override int WriteAdditionalTags(ref byte[] bytes, ref int offset, ITagProcessor[] tagProcessors)
        {
            var count = 0;
            if (QueueName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, QueueNameBytes, QueueName, tagProcessors);
            }

            if (QueueUrl != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, QueueUrlBytes, QueueUrl, tagProcessors);
            }

            if (SpanKind != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, SpanKindBytes, SpanKind, tagProcessors);
            }

            return count + base.WriteAdditionalTags(ref bytes, ref offset, tagProcessors);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (QueueName != null)
            {
                sb.Append("aws.queue.name (tag):")
                  .Append(QueueName)
                  .Append(',');
            }

            if (QueueUrl != null)
            {
                sb.Append("aws.queue.url (tag):")
                  .Append(QueueUrl)
                  .Append(',');
            }

            if (SpanKind != null)
            {
                sb.Append("span.kind (tag):")
                  .Append(SpanKind)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
