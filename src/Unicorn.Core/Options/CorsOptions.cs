using Microsoft.Extensions.Primitives;

namespace Unicorn.Options
{
    public class CorsOptions : IOptions
    {
        public bool IsEnabled { get; set; } = false;
        public StringValues Origins { get; set; } = "*";
        public StringValues Headers { get; set; } = new[] { "Origin", "X-Requested-With", "Content-Type", "Accept", "Authorization" };
        public StringValues Methods { get; set; } = new[] { "GET", "HEAD", "POST", "PUT", "PATCH", "DELETE", "OPTIONS", "TRACES" };
        public bool Credentials { get; set; } = true;
    }
}
