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
    public class UnicornRequestMiddleware : UnicornMiddlewareBase<RequestOptions>
    {
        public UnicornRequestMiddleware(
            UnicornContext context, IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.RequestOptions, context, unicornOptions)
        {
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = UnicornContext.RequestData;
            if (Options?.IsEnabled == true)
            {
                AddHeaders(request);
                AddHeadersFromRoutes(request);
                AddHeadersFromForms(request);
                AddHeadersFromQueries(request);
                HeaderTransform(request);
                AddQueries(request);
                AddQueriesFromRoute(request);
                AddQueriesFromForms(request);
                AddQueriesFromHeaders(request);
                QueryTransform(request);
                AddForms(request);
                AddFormsFromRoute(request);
                AddFormsFromHeaders(request);
                AddFormsFromQueries(request);
                FormTransform(request);
            }
            var fromIp = context.Connection.RemoteIpAddress.ToString();
            if (request.Headers.ContainsKey("X-Forwarded-For"))
            {
                var list = request.Headers["X-Forwarded-For"].ToList();
                list.Add(fromIp);
                request.Headers["X-Forwarded-For"] = list.ToArray();
                if (!request.Headers.ContainsKey("X-Real-IP"))
                {
                    request.Headers["X-Real-IP"] = list.First();
                }
            }
            else if (!request.Headers.ContainsKey("X-Real-IP"))
            {
                request.Headers["X-Forwarded-For"] = fromIp;
                request.Headers["X-Real-IP"] = fromIp;
            }
            else
            {
                request.Headers["X-Forwarded-For"] = new[] { request.Headers["X-Real-IP"].ToString(), fromIp };
            }
            //TODO:发起下游网络请求

            await next(context);
        }

        protected void AddHeaders(RequestData request)
        {
            if (Options.AddHeaders == null || Options.AddHeaders.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeaders)
            {
                request.Headers[header.Key] = header.Value;
            }
        }

        protected void AddHeadersFromRoutes(RequestData request)
        {
            if (Options.AddHeadersFromRoutes == null || Options.AddHeadersFromRoutes.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeadersFromRoutes)
            {
                if (!UnicornContext.RouteData.ContainsKey(header.Value)) continue;
                var routeValue = UnicornContext.RouteData[header.Value]?.ToString();
                request.Headers[header.Key] = routeValue;
            }
        }

        protected void AddHeadersFromForms(RequestData request)
        {
            if (Options.AddHeadersFromForms == null || Options.AddHeadersFromForms.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeadersFromForms)
            {
                if (!request.Form.ContainsKey(header.Value)) continue;
                var formValue = request.Form[header.Value];
                request.Headers[header.Key] = formValue;
            }
        }

        protected void AddHeadersFromQueries(RequestData request)
        {
            if (Options.AddHeadersFromQueries == null || Options.AddHeadersFromQueries.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeadersFromQueries)
            {
                if (!request.Query.ContainsKey(header.Value)) continue;
                var queryValue = request.Query[header.Value];
                request.Headers[header.Key] = queryValue;
            }
        }

        protected void HeaderTransform(RequestData request)
        {
            if (Options.HeaderTransform == null || Options.HeaderTransform.Count == 0)
            {
                return;
            }
            foreach (var header in Options.HeaderTransform)
            {
                var value = request.Headers[header.Key];
                request.Headers.Remove(header.Key);
                request.Headers[header.Value] = value;
            }
        }

        protected void AddQueries(RequestData request)
        {
            if (Options.AddQueries == null || Options.AddQueries.Count == 0)
            {
                return;
            }
            foreach (var query in Options.AddQueries)
            {
                request.Query[query.Key] = query.Value;
            }
        }

        protected void AddQueriesFromRoute(RequestData request)
        {
            if (Options.AddQueriesFromRoute == null || Options.AddQueriesFromRoute.Count == 0)
            {
                return;
            }
            foreach (var query in Options.AddQueriesFromRoute)
            {
                if (!UnicornContext.RouteData.ContainsKey(query.Value)) continue;
                var routeValue = UnicornContext.RouteData[query.Value]?.ToString();
                request.Query[query.Key] = routeValue;
            }
        }

        protected void AddQueriesFromForms(RequestData request)
        {
            if (Options.AddQueriesFromForms == null || Options.AddQueriesFromForms.Count == 0)
            {
                return;
            }
            foreach (var query in Options.AddQueriesFromForms)
            {
                if (!request.Form.ContainsKey(query.Value)) continue;
                var routeValue = request.Form[query.Value].ToString();
                request.Query[query.Key] = routeValue;
            }
        }

        protected void AddQueriesFromHeaders(RequestData request)
        {
            if (Options.AddQueriesFromForms == null || Options.AddQueriesFromForms.Count == 0)
            {
                return;
            }
            foreach (var query in Options.AddQueriesFromForms)
            {
                if (!UnicornContext.RequestData.Headers.ContainsKey(query.Value)) continue;
                var headerValue = UnicornContext.RequestData.Headers[query.Value].ToString();
                request.Query[query.Key] = headerValue;
            }
        }

        protected void QueryTransform(RequestData request)
        {
            if (Options.QueryTransform == null || Options.QueryTransform.Count == 0)
            {
                return;
            }
            foreach (var query in Options.QueryTransform)
            {
                var value = request.Query[query.Key];
                request.Query.Remove(query.Key);
                request.Query[query.Value] = value;
            }
        }

        protected void AddForms(RequestData request)
        {
            if (Options.AddForms == null || Options.AddForms.Count == 0)
            {
                return;
            }
            foreach (var form in Options.AddForms)
            {
                request.Form[form.Key] = form.Value;
            }
        }

        protected void AddFormsFromRoute(RequestData request)
        {
            if (Options.AddFormsFromRoute == null || Options.AddFormsFromRoute.Count == 0)
            {
                return;
            }
            foreach (var form in Options.AddFormsFromRoute)
            {
                if (!UnicornContext.RouteData.ContainsKey(form.Value)) continue;
                var routeValue = UnicornContext.RouteData[form.Value]?.ToString();
                request.Form[form.Key] = routeValue;
            }
        }

        protected void AddFormsFromHeaders(RequestData request)
        {
            if (Options.AddQueriesFromForms == null || Options.AddQueriesFromForms.Count == 0)
            {
                return;
            }
            foreach (var form in Options.AddQueriesFromForms)
            {
                if (!UnicornContext.RequestData.Headers.ContainsKey(form.Value)) continue;
                var headerValue = UnicornContext.RequestData.Headers[form.Value].ToString();
                request.Form[form.Key] = headerValue;
            }
        }

        protected void AddFormsFromQueries(RequestData request)
        {
            if (Options.AddFormsFromQueries == null || Options.AddFormsFromQueries.Count == 0)
            {
                return;
            }
            foreach (var form in Options.AddFormsFromQueries)
            {
                if (!request.Query.ContainsKey(form.Value)) continue;
                var queryValue = request.Query[form.Value];
                request.Headers[form.Key] = queryValue;
            }
        }

        protected void FormTransform(RequestData request)
        {
            if (Options.FormTransform == null || Options.FormTransform.Count == 0)
            {
                return;
            }
            foreach (var form in Options.FormTransform)
            {
                var value = request.Form[form.Key];
                request.Form.Remove(form.Key);
                request.Form[form.Value] = value;
            }
        }
    }
}
