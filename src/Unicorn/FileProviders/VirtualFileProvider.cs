using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.FileProviders
{
    public class VirtualFileProvider : IFileProvider
    {
        public const string ProviderName = "Virtual";
        public string Name => ProviderName;
    }
}
