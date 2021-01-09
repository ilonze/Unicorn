using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Dictionary<string, string> AddQueriesFromRoute { get; set; }
        public Dictionary<string, string> AddQueriesFromForms { get; set; }
        public Dictionary<string, string> AddQueriesFromHeaders { get; set; }
        public Dictionary<string, string> QueryTransform { get; set; }
        public Dictionary<string, StringValues> AddForms { get; set; }
        public Dictionary<string, string> AddFormsFromRoute { get; set; }
        public Dictionary<string, string> AddFormsFromHeaders { get; set; }
        public Dictionary<string, string> AddFormsFromQueries { get; set; }
        public Dictionary<string, string> FormTransform { get; set; }
    }
}
