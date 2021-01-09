using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ProviderNameAttribute: Attribute
    {
        public string Name { get; }
        public ProviderNameAttribute(string name)
        {
            Name = name;
        }
    }
}
