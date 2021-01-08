using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class AuthenticationOptions
    {
        public bool IsEnabled { get; set; } = false;
        public AuthenticationOptions()
        {
            AllowedScopes = new List<string>();
        }

        public string AuthenticationProviderKey { get; set; }
        public List<string> AllowedScopes { get; set; }
    }
}
