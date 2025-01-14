﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;
using Datadog.Trace.Tagging;

namespace Datadog.Trace.Tagging
{
    partial class AwsSdkTags
    {
        // InstrumentationNameBytes = System.Text.Encoding.UTF8.GetBytes("component");
        private static readonly byte[] InstrumentationNameBytes = new byte[] { 99, 111, 109, 112, 111, 110, 101, 110, 116 };
        // AgentNameBytes = System.Text.Encoding.UTF8.GetBytes("aws.agent");
        private static readonly byte[] AgentNameBytes = new byte[] { 97, 119, 115, 46, 97, 103, 101, 110, 116 };
        // OperationBytes = System.Text.Encoding.UTF8.GetBytes("aws.operation");
        private static readonly byte[] OperationBytes = new byte[] { 97, 119, 115, 46, 111, 112, 101, 114, 97, 116, 105, 111, 110 };
        // RegionBytes = System.Text.Encoding.UTF8.GetBytes("aws.region");
        private static readonly byte[] RegionBytes = new byte[] { 97, 119, 115, 46, 114, 101, 103, 105, 111, 110 };
        // RequestIdBytes = System.Text.Encoding.UTF8.GetBytes("aws.requestId");
        private static readonly byte[] RequestIdBytes = new byte[] { 97, 119, 115, 46, 114, 101, 113, 117, 101, 115, 116, 73, 100 };
        // ServiceBytes = System.Text.Encoding.UTF8.GetBytes("aws.service");
        private static readonly byte[] ServiceBytes = new byte[] { 97, 119, 115, 46, 115, 101, 114, 118, 105, 99, 101 };
        // HttpMethodBytes = System.Text.Encoding.UTF8.GetBytes("http.method");
        private static readonly byte[] HttpMethodBytes = new byte[] { 104, 116, 116, 112, 46, 109, 101, 116, 104, 111, 100 };
        // HttpUrlBytes = System.Text.Encoding.UTF8.GetBytes("http.url");
        private static readonly byte[] HttpUrlBytes = new byte[] { 104, 116, 116, 112, 46, 117, 114, 108 };
        // HttpStatusCodeBytes = System.Text.Encoding.UTF8.GetBytes("http.status_code");
        private static readonly byte[] HttpStatusCodeBytes = new byte[] { 104, 116, 116, 112, 46, 115, 116, 97, 116, 117, 115, 95, 99, 111, 100, 101 };

        public override string? GetTag(string key)
        {
            return key switch
            {
                "component" => InstrumentationName,
                "aws.agent" => AgentName,
                "aws.operation" => Operation,
                "aws.region" => Region,
                "aws.requestId" => RequestId,
                "aws.service" => Service,
                "http.method" => HttpMethod,
                "http.url" => HttpUrl,
                "http.status_code" => HttpStatusCode,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "aws.operation": 
                    Operation = value;
                    break;
                case "aws.region": 
                    Region = value;
                    break;
                case "aws.requestId": 
                    RequestId = value;
                    break;
                case "aws.service": 
                    Service = value;
                    break;
                case "http.method": 
                    HttpMethod = value;
                    break;
                case "http.url": 
                    HttpUrl = value;
                    break;
                case "http.status_code": 
                    HttpStatusCode = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        public override void EnumerateTags<TProcessor>(ref TProcessor processor)
        {
            if (InstrumentationName is not null)
            {
                processor.Process(new TagItem<string>("component", InstrumentationName, InstrumentationNameBytes));
            }

            if (AgentName is not null)
            {
                processor.Process(new TagItem<string>("aws.agent", AgentName, AgentNameBytes));
            }

            if (Operation is not null)
            {
                processor.Process(new TagItem<string>("aws.operation", Operation, OperationBytes));
            }

            if (Region is not null)
            {
                processor.Process(new TagItem<string>("aws.region", Region, RegionBytes));
            }

            if (RequestId is not null)
            {
                processor.Process(new TagItem<string>("aws.requestId", RequestId, RequestIdBytes));
            }

            if (Service is not null)
            {
                processor.Process(new TagItem<string>("aws.service", Service, ServiceBytes));
            }

            if (HttpMethod is not null)
            {
                processor.Process(new TagItem<string>("http.method", HttpMethod, HttpMethodBytes));
            }

            if (HttpUrl is not null)
            {
                processor.Process(new TagItem<string>("http.url", HttpUrl, HttpUrlBytes));
            }

            if (HttpStatusCode is not null)
            {
                processor.Process(new TagItem<string>("http.status_code", HttpStatusCode, HttpStatusCodeBytes));
            }

            base.EnumerateTags(ref processor);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (InstrumentationName is not null)
            {
                sb.Append("component (tag):")
                  .Append(InstrumentationName)
                  .Append(',');
            }

            if (AgentName is not null)
            {
                sb.Append("aws.agent (tag):")
                  .Append(AgentName)
                  .Append(',');
            }

            if (Operation is not null)
            {
                sb.Append("aws.operation (tag):")
                  .Append(Operation)
                  .Append(',');
            }

            if (Region is not null)
            {
                sb.Append("aws.region (tag):")
                  .Append(Region)
                  .Append(',');
            }

            if (RequestId is not null)
            {
                sb.Append("aws.requestId (tag):")
                  .Append(RequestId)
                  .Append(',');
            }

            if (Service is not null)
            {
                sb.Append("aws.service (tag):")
                  .Append(Service)
                  .Append(',');
            }

            if (HttpMethod is not null)
            {
                sb.Append("http.method (tag):")
                  .Append(HttpMethod)
                  .Append(',');
            }

            if (HttpUrl is not null)
            {
                sb.Append("http.url (tag):")
                  .Append(HttpUrl)
                  .Append(',');
            }

            if (HttpStatusCode is not null)
            {
                sb.Append("http.status_code (tag):")
                  .Append(HttpStatusCode)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
