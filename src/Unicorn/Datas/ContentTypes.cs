using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Datas
{
    public class ContentTypes
    {
        public class Application
        {
            public const string Json = "application/json";
            public const string Xml = "application/xml";
            public const string Yaml = "application/yaml";
            public const string Text = "text/plain";
            public const string OctetStream = "octet-stream";
        }

        public class Text
        {
            public const string Json = "text/json";
            public const string Javascript = "text/javascript";
            public const string Xml = "text/xml";
            public const string Html = "text/html";
            public const string Plain = "text/plain";
        }
    }
}
