using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class ResponseOptions : IOptions
    {
        public bool IsEnabled { get; set; }
        public Dictionary<string, StringValues> AddHeaders { get; set; }
        public Dictionary<string, string> AddHeadersFromRoutes { get; set; }
        public Dictionary<string, string> AddHeadersFromForms { get; set; }
        public Dictionary<string, string> AddHeadersFromQueries { get; set; }
        public Dictionary<string, string> HeaderTransform { get; set; }
        public Dictionary<int, int> HttpStatusCodeTransform { get; set; }
        public string ServerHeader { get; set; }
    }
}
