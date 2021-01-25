using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Options;

namespace Unicorn.Handlers
{
    public class RouteHandler : IRouteHandler
    {
        private Dictionary<string, RouteRule> _routeUpstreamDictionary = new Dictionary<string, RouteRule>();

        private readonly Dictionary<string, TemplateBinder> _routeDownstreamDictionary
            = new Dictionary<string, TemplateBinder>();

        private readonly List<TemplateMatcher> _routeUpstreamTemplates
            = new List<TemplateMatcher>();

        private readonly Dictionary<string, TemplateMatcher> _routeUpstreamTemplateDictionary
            = new Dictionary<string, TemplateMatcher>();

        private readonly TemplateBinderFactory _templateBinderFactory;

        private readonly UnicornOptions _options;

        //private readonly IUnicornCacheManager _unicornCacheManager;

        public RouteHandler(
            TemplateBinderFactory templateBinderFactory,
            IOptions<UnicornOptions> options
            //,
            //IUnicornCacheManager unicornCacheManager
            )
        {
            _templateBinderFactory = templateBinderFactory;
            _options = options.Value;
            //_unicornCacheManager = unicornCacheManager;
        }

        private void ParseDownstreamRoute(DownstreamRoute route)
        {
            var template = TemplateParser.Parse(route.DownstreamRouteTemplate);
            var binder = _templateBinderFactory.Create(template, GetDefaults(template));
            _routeDownstreamDictionary[route.DownstreamRouteTemplate] = binder;
            if (route.DownstreamFactors == null || route.DownstreamFactors.Count == 0)
            {
                route.DownstreamFactors ??= new List<DownstreamFactor>();
                route.DownstreamFactors.Add(route);
            }
        }

        public void Initialization()
        {
            foreach (var route in _options.RouteRules)
            {
                AddRoute(route);
            }
            _routeUpstreamTemplates.Clear();
            _routeUpstreamTemplates.AddRange(
                _options.RouteRules
                    .OrderBy(r => r.Priority)
                    .Select(r => _routeUpstreamTemplateDictionary[r.UpstreamRouteTemplate])
                );
        }

        public void AddRouteRule(RouteRule route)
        {
            AddRoute(route);
            _options.RouteRules.Add(route);
            _routeUpstreamTemplates.Clear();
            _routeUpstreamTemplates.AddRange(
                _routeUpstreamDictionary.Values
                    .OrderBy(r => r.Priority)
                    .Select(r => _routeUpstreamTemplateDictionary[r.UpstreamRouteTemplate])
                );
        }

        private void AddRoute(RouteRule route)
        {
            var template = TemplateParser.Parse(route.UpstreamRouteTemplate);
            var matcher = new TemplateMatcher(template, GetDefaults(template));
            _routeUpstreamTemplateDictionary[route.UpstreamRouteTemplate] = matcher;

            if (route.DownstreamRoutes == null || route.DownstreamRoutes.Count == 0)
            {
                var dsRoute = (DownstreamRoute)route;
                ParseDownstreamRoute(dsRoute);
                route.DownstreamRoutes ??= new List<DownstreamRoute>();
                route.DownstreamRoutes.Add(dsRoute);
            }
            else
            {
                foreach (var downstreamRoute in route.DownstreamRoutes)
                {
                    ParseDownstreamRoute(downstreamRoute);
                }
            }
            _routeUpstreamDictionary[route.Key] = route;

        }

        public RouteValueDictionary MatchRequestPath(HttpContext context, out RouteRule route)
        {
            var requestPath = context.Request.Path.Value;
            var routeValue = new RouteValueDictionary();
            foreach (var templateMatcher in _routeUpstreamTemplates)
            {
                if (templateMatcher.TryMatch(requestPath, routeValue))
                {
                    route = _routeUpstreamDictionary[templateMatcher.Template.TemplateText];
                    if (route.UpstreamFactors.Any())
                    {
                        foreach (var factor in route.UpstreamFactors)
                        {
                            if((string.IsNullOrEmpty(factor.HttpMethod) || factor.HttpMethod.Equals(context.Request.Method, StringComparison.CurrentCultureIgnoreCase))
                                && (string.IsNullOrEmpty(factor.Host) || factor.Host.Equals(context.Request.Host.Host))
                                && (string.IsNullOrEmpty(factor.Schema) || factor.Schema.Equals(context.Request.Scheme))
                                && (factor.Port == default || factor.Port == context.Request.Host.Port))
                            {
                                return routeValue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    return routeValue;
                }
            }
            route = null;
            return null;
        }

        private static RouteValueDictionary GetDefaults(RouteTemplate parsedTemplate)
        {
            var result = new RouteValueDictionary();
            foreach (var parameter in parsedTemplate.Parameters)
            {
                if (parameter.DefaultValue != null)
                {
                    result.Add(parameter.Name, parameter.DefaultValue);
                }
            }
            return result;
        }

        public string FillRoute(string key, RouteValueDictionary routeData)
        {
            if (_routeDownstreamDictionary.TryGetValue(key, out var binder))
            {
                return binder.BindValues(routeData);
            }
            return null;
        }

        public void DeleteRouteRule(string routeKey)
        {
            if (_routeUpstreamDictionary.ContainsKey(routeKey))
            {
                var route = _routeUpstreamDictionary[routeKey];
                _options.RouteRules.Remove(route);
                //TODO:
                _routeDownstreamDictionary.Remove(routeKey);
            }
        }

        public void SetRouteRule(RouteRule route)
        {
            if (_routeUpstreamDictionary.ContainsKey(route.Key))
            {
                _options.RouteRules.Remove(_routeUpstreamDictionary[route.Key]);
            }
            AddRoute(route);
        }
    }
}
