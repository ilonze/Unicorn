using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Unicorn.Options
{
    public class RequestOptions : IOptions
    {
        public bool IsEnabled { get; set; }
        public Dictionary<string, StringValues> AddHeaders { get; set; }
        public Dictionary<string, string> AddHeadersFromRoutes { get; set; }
        public Dictionary<string, string> AddHeadersFromForms { get; set; }
        public Dictionary<string, string> AddHeadersFromQueries { get; set; }
        public Dictionary<string, string> HeaderTransform { get; set; }
        public Dictionary<string, StringValues> AddQueries { get; set; }
        public Dictionary<string, string> AddQueriesFromRoutes { get; set; }
        public Dictionary<string, string> AddQueriesFromForms { get; set; }
        public Dictionary<string, string> AddQueriesFromHeaders { get; set; }
        public Dictionary<string, string> QueryTransform { get; set; }
        public Dictionary<string, StringValues> AddForms { get; set; }
        public Dictionary<string, string> AddFormsFromRoutes { get; set; }
        public Dictionary<string, string> AddFormsFromHeaders { get; set; }
        public Dictionary<string, string> AddFormsFromQueries { get; set; }
        public Dictionary<string, string> FormTransform { get; set; }
        public Dictionary<string, string> AddRoutes { get; set; }
        public Dictionary<string, string> AddRoutesFromHeaders { get; set; }
        public Dictionary<string, string> AddRoutesFromQueries { get; set; }
        public Dictionary<string, string> AddRoutesFromForms { get; set; }
        public Dictionary<string, string> RouteTransform { get; set; }
        public int Timeout { get; set; }
    }
}
