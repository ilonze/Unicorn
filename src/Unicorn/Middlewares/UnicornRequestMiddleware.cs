using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;
using IRouteHandler = Unicorn.Handlers.IRouteHandler;

namespace Unicorn.Middlewares
{
    public class UnicornRequestMiddleware : UnicornMiddlewareBase<RequestOptions>
    {
        protected IRouteHandler RouteHandler { get; }
        protected ILogger<UnicornRequestMiddleware> Logger { get; }
        public UnicornRequestMiddleware(
            UnicornContext context,
            IRouteHandler routeHandler,
            IOptions<UnicornOptions> unicornOptions,
            ILogger<UnicornRequestMiddleware> logger)
            : base(context.RouteRule.RequestOptions, context, unicornOptions)
        {
            RouteHandler = routeHandler;
            Logger = logger;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = UnicornContext.RequestData;
            if (Options?.IsEnabled == true)
            {
                AddHeaders(request);
                AddHeadersFromRoutes(request, UnicornContext.RouteData);
                AddHeadersFromForms(request);
                AddHeadersFromQueries(request);
                HeaderTransform(request);
                AddQueries(request);
                AddQueriesFromRoute(request, UnicornContext.RouteData);
                AddQueriesFromForms(request);
                AddQueriesFromHeaders(request);
                QueryTransform(request);
                AddForms(request);
                AddFormsFromRoutes(request, UnicornContext.RouteData);
                AddFormsFromHeaders(request);
                AddFormsFromQueries(request);
                FormTransform(request);
                AddRoutes(UnicornContext.RouteData);
                AddRoutesFromForms(request, UnicornContext.RouteData);
                AddRoutesFromHeaders(request, UnicornContext.RouteData);
                AddRoutesFromQueries(request, UnicornContext.RouteData);
                RouteTransform(UnicornContext.RouteData);
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

            var queryString = request.Query.Count > 0 ? ("?" + request.Query.Select(r => r.Key + "=" + r.Value).JoinAsString("&").UrlEncode()) : "";

            var content = await GetContentAsync(request);

            var timeout = Options.Timeout > 0 ? Options.Timeout : -1;

            var routes = UnicornContext.DownstreamRoutes;
            var tasks = new Dictionary<string, Task<HttpResponseMessage>>();
            foreach (var route in routes)
            {
                var path = RouteHandler.FillRoute(route.DownstreamRouteTemplate, UnicornContext.RouteData);
                var serviceName = route.DownstreamServiceName;
                var service = UnicornContext.Services[serviceName];
                var url = $"{route.DownstreamSchema}://{service.Host}:{service.Port}{path}{queryString}";
                tasks[serviceName] = RequestDownstreamAsync(url, route, content, timeout, context.RequestAborted);
            }
            Task.WaitAll(tasks.Values.ToArray(), timeout, context.RequestAborted);
            UnicornContext.ResponseMessages = tasks.ToDictionary(r => r.Key, r => r.Value.Result);
            UnicornContext.Exceptions = tasks.ToDictionary(r => r.Key, r => r.Value.Exception);
            await next(context);
        }

        protected async Task<HttpContent> GetContentAsync(RequestData request)
        {
            HttpContent content;
            if (request.HasFormContentType || request.HasFormDataContentType)
            {
                content = new FormUrlEncodedContent(request.Form.ToDictionary(r => r.Key, r => r.Value.ToString()));
                if (request.HasFormDataContentType)
                {
                    var form = new MultipartFormDataContent
                    {
                        content
                    };
                    foreach (var file in request.Files)
                    {
                        using var stream = new MemoryStream();
                        await file.CopyToAsync(stream);
                        var fileContent = new ByteArrayContent(stream.ToArray());
                        form.Add(fileContent, file.Name, file.FileName);
                    }
                    content = form;
                }
            }
            else if (request.HasJsonContentType)
            {
                content = new StringContent(request.Json, Encoding.UTF8, request.ContentType);
            }
            else if (request.HasTextContentType)
            {
                content = new StringContent(request.Text, Encoding.UTF8, request.ContentType);
            }
            else
            {
                content = new ByteArrayContent(request.Body);
            }
            foreach (var header in request.Headers)
            {
                try
                {
                    content.Headers.Add(header.Key, header.Value.ToArray());
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, $"Add header fail: {header.Key}={header.Value}");
                }
            }
            return content;
        }

        protected async Task<HttpResponseMessage> RequestDownstreamAsync(string url, DownstreamRoute route, HttpContent content, int timeout, CancellationToken token = default)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod(route.DownstreamHttpMethod), url)
            {
                Content = content
            };
            if (timeout > 0)
            {
                client.Timeout = TimeSpan.FromMilliseconds(timeout);
            }
            var message = await client.SendAsync(request, token);
            return message;
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

        protected void AddHeadersFromRoutes(RequestData request, RouteValueDictionary routeData)
        {
            if (Options.AddHeadersFromRoutes == null || Options.AddHeadersFromRoutes.Count == 0)
            {
                return;
            }
            foreach (var header in Options.AddHeadersFromRoutes)
            {
                if (!routeData.ContainsKey(header.Value)) continue;
                var routeValue = routeData[header.Value]?.ToString();
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

        protected void AddQueriesFromRoute(RequestData request, RouteValueDictionary routeData)
        {
            if (Options.AddQueriesFromRoutes == null || Options.AddQueriesFromRoutes.Count == 0)
            {
                return;
            }
            foreach (var query in Options.AddQueriesFromRoutes)
            {
                if (!routeData.ContainsKey(query.Value)) continue;
                var routeValue = routeData[query.Value]?.ToString();
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
                if (!request.Headers.ContainsKey(query.Value)) continue;
                var headerValue = request.Headers[query.Value].ToString();
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

        protected void AddFormsFromRoutes(RequestData request, RouteValueDictionary routeData)
        {
            if (Options.AddFormsFromRoutes == null || Options.AddFormsFromRoutes.Count == 0)
            {
                return;
            }
            foreach (var form in Options.AddFormsFromRoutes)
            {
                if (!routeData.ContainsKey(form.Value)) continue;
                var routeValue = routeData[form.Value]?.ToString();
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
                if (!request.Headers.ContainsKey(form.Value)) continue;
                var headerValue = request.Headers[form.Value].ToString();
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
                request.Form[form.Key] = queryValue;
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

        protected void AddRoutes(RouteValueDictionary routeData)
        {
            if (Options.AddRoutes == null || Options.AddRoutes.Count == 0)
            {
                return;
            }
            foreach (var route in Options.AddRoutes)
            {
                routeData[route.Key] = route.Value;
            }
        }

        protected void AddRoutesFromForms(RequestData request, RouteValueDictionary routeData)
        {
            if (Options.AddRoutesFromForms == null || Options.AddRoutesFromForms.Count == 0)
            {
                return;
            }
            foreach (var route in Options.AddRoutesFromForms)
            {
                if (!request.Form.ContainsKey(route.Value)) continue;
                var formValue = request.Form[route.Value].ToString();
                routeData[route.Key] = formValue;
            }
        }

        protected void AddRoutesFromHeaders(RequestData request, RouteValueDictionary routeData)
        {
            if (Options.AddRoutesFromHeaders == null || Options.AddRoutesFromHeaders.Count == 0)
            {
                return;
            }
            foreach (var route in Options.AddRoutesFromHeaders)
            {
                if (!request.Headers.ContainsKey(route.Value)) continue;
                var headerValue = request.Headers[route.Value].ToString();
                routeData[route.Key] = headerValue;
            }
        }

        protected void AddRoutesFromQueries(RequestData request, RouteValueDictionary routeData)
        {
            if (Options.AddRoutesFromQueries == null || Options.AddRoutesFromQueries.Count == 0)
            {
                return;
            }
            foreach (var route in Options.AddRoutesFromQueries)
            {
                if (!request.Query.ContainsKey(route.Value)) continue;
                var queryValue = request.Query[route.Value];
                routeData[route.Key] = queryValue;
            }
        }

        protected void RouteTransform(RouteValueDictionary routeData)
        {
            if (Options.RouteTransform == null || Options.RouteTransform.Count == 0)
            {
                return;
            }
            foreach (var route in Options.RouteTransform)
            {
                var value = routeData[route.Key];
                routeData.Remove(route.Key);
                routeData[route.Value] = value;
            }
        }
    }
}
