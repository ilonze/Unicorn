using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class SignOptions : IOptions
    {
        public bool IsEnabled { get; set; } = false;
        public string ProviderName { get; set; }
    }
}
