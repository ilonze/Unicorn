using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Providers.Files
{
    [ProviderName(ProviderName)]
    public class StaticFileProvider : IFileProvider
    {
        public const string ProviderName = "Static";
        public string Name => ProviderName;
    }
}
