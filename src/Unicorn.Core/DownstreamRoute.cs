//using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class DownstreamRoute
    {
        public string DownstreamRouteTemplate { get; set; }
        //internal TemplateBinder DownstreamRouteTemplateBinder { get; set; }
        public List<DownstreamFactor> DownstreamFactors { get; set; }
        public string DownstreamServiceNamespace { get; set; }
        public string DownstreamServiceName { get; set; }
        public string DownstreamSchema { get; set; }
        public string DownstreamHttpMethod { get; set; }
        public static implicit operator DownstreamFactor(DownstreamRoute dsRoute)
        {
            return new DownstreamFactor
            {
                HttpMethod = dsRoute.DownstreamHttpMethod,
                Schema = dsRoute.DownstreamSchema,
                ServiceName = dsRoute.DownstreamServiceName,
                ServiceNamespace = dsRoute.DownstreamServiceNamespace
            };
        }
    }
}
