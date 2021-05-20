#if !NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datadog.Trace.DuckTyping;
using Datadog.Trace.Logging;
using Microsoft.AspNetCore.Http;

namespace Datadog.Trace.AppSec.AspNetCore
{
    internal static class BlockingMiddleware
    {
        private static readonly IDatadogLogger Log = DatadogLogging.GetLoggerFor(typeof(BlockingMiddleware));

        public static void ModifyASpplicationBuilder(object applicationBuilderInstance)
        {
            if (applicationBuilderInstance.TryDuckCast<ApplicationBuilderDuck>(out var applicationBuilder))
            {
                InsertMiddlewares(applicationBuilder.Components);
            }
            else
            {
                Log.Error($"Couldn't create duck type from {applicationBuilderInstance?.GetType()?.FullName ?? "(null)"}");
            }
        }

        private static void InsertMiddlewares(List<Func<RequestDelegate, RequestDelegate>> components)
        {
            Func<HttpContext, Func<Task>, Task> middleware = async (context, next) =>
            {
                if (context.Items.ContainsKey(SecurityConstants.KillKey) && context.Items[SecurityConstants.KillKey] is bool killKey && killKey)
                {
                    await BlockRequest(context);
                }
                else
                {
                    try
                    {
                        context.Items[SecurityConstants.InHttpPipeKey] = true;
                        await next.Invoke();
                    }
                    catch (BlockActionException)
                    {
                        await BlockRequest(context);
                    }
                }
            };

            Func<RequestDelegate, RequestDelegate> middlewareWrapper = next =>
            {
                return context =>
                {
                    Func<Task> simpleNext = () => next(context);
                    return middleware(context, simpleNext);
                };
            };

            components.Insert(0, middlewareWrapper);
            if (components.Count > 2)
            {
                // insert 2nd to last, making a guess that the last one will be the user action
                components.Insert(components.Count - 2, middlewareWrapper);
            }
        }

        private static async Task BlockRequest(HttpContext context)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(SecurityConstants.AttackBlockedHtml);
        }

        /// <summary>
        /// Application builder proxy
        /// </summary>
        [DuckCopy]
        public struct ApplicationBuilderDuck
        {
            /// <summary>
            /// The components that will make up the application http pipe
            /// </summary>
            [Duck(Name = "_components", Kind = DuckKind.Field)]
            public List<Func<RequestDelegate, RequestDelegate>> Components;
        }
    }
}
#endif
