using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornResponseMiddleware: UnicornMiddlewareBase<ResponseOptions>
    {
        public UnicornResponseMiddleware(UnicornContext context, IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.ResponseOptions, context, unicornOptions)
        {

        }

        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled == true)
            {
                var request = UnicornContext.RequestData;
                var response = UnicornContext.ResponseData;
                AddHeaders(response);
                AddHeadersFromRoutes(response);
                AddHeadersFromForms(request, response);
                AddHeadersFromQueries(request, response);
                HeaderTransform(response);
                HttpStatusCodeTransform(response);
                if (!string.IsNullOrEmpty(Options.ServerHeader))
                {
                    response.Headers["Server"] = Options.ServerHeader;
                }
                else
                {
                    response.Headers["Server"] = "Unicorn";
                }
                var list = new List<string>();
                if (response.Headers.ContainsKey("X-Power-By"))
                {
                    list.AddRange(response.Headers["X-Power-By"]);
                }
                list.Add("Unicorn");
                response.Headers["X-Power-By"] = list.ToArray();
            }
            await next(context);
        }

        protected void HeaderTransform(ResponseData response)
        {
            if (Options.HeaderTransform == null || Options.HeaderTransform.Count == 0)
            {
                return;
            }
            foreach (var header in Options.HeaderTransform)
            {
                var value = response.Headers[header.Key];
                response.Headers.Remove(header.Key);
                response.Headers[header.Value] = value;
            }
        }

        protected void AddHeaders(ResponseData response)
        {
            if (Options.AddHeaders == null || Options.AddHeaders.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeaders)
            {
                response.Headers[header.Key] = header.Value;
            }
        }

        protected void AddHeadersFromRoutes(ResponseData response)
        {
            if (Options.AddHeadersFromRoutes == null || Options.AddHeadersFromRoutes.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeadersFromRoutes)
            {
                if (!UnicornContext.RouteData.ContainsKey(header.Value)) continue;
                var routeValue = UnicornContext.RouteData[header.Value]?.ToString();
                response.Headers[header.Key] = routeValue;
            }
        }

        protected void AddHeadersFromForms(RequestData request, ResponseData response)
        {
            if (Options.AddHeadersFromForms == null || Options.AddHeadersFromForms.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeadersFromForms)
            {
                if (!request.Form.ContainsKey(header.Value)) continue;
                var formValue = request.Form[header.Value];
                response.Headers[header.Key] = formValue;
            }
        }

        protected void AddHeadersFromQueries(RequestData request, ResponseData response)
        {
            if (Options.AddHeadersFromQueries == null || Options.AddHeadersFromQueries.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeadersFromQueries)
            {
                if (!request.Query.ContainsKey(header.Value)) continue;
                var queryValue = request.Query[header.Value];
                response.Headers[header.Key] = queryValue;
            }
        }

        protected void HttpStatusCodeTransform(ResponseData response)
        {
            if (Options.HttpStatusCodeTransform == null
                || Options.HttpStatusCodeTransform.Count == 0
                || !Options.HttpStatusCodeTransform.ContainsKey(response.StatusCode)
                )
            {
                return;
            }
            response.StatusCode = Options.HttpStatusCodeTransform[response.StatusCode];
        }
    }
}
