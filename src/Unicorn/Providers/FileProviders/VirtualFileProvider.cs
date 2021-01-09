using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Providers.Files
{
    [ProviderName(ProviderName)]
    public class VirtualFileProvider : IFileProvider
    {
        public const string ProviderName = "Virtual";
        public string Name => ProviderName;
    }
}
