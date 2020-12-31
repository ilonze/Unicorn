using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.FileProviders
{
    public class StaticFileProvider : IFileProvider
    {
        public const string ProviderName = "Static";
        public string Name => ProviderName;
    }
}
