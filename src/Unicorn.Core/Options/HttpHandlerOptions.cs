using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class HttpHandlerOptions : IOptions
    {
        public HttpHandlerOptions()
        {
            AllowAutoRedirect = false;
            UseCookieContainer = false;
            UseProxy = true;
            MaxConnectionsPerServer = int.MaxValue;
        }

        public bool IsEnabled { get; set; } = false;

        public bool AllowAutoRedirect { get; set; }

        public bool UseCookieContainer { get; set; }

        public bool UseTracing { get; set; }

        public bool UseProxy { get; set; }

        public int MaxConnectionsPerServer { get; set; }
    }
}
