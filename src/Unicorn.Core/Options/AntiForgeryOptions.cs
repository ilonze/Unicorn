using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class AntiForgeryOptions: IOptions
    {
        public bool IsEnabled { get; set; } = false;
        public string CookieName { get; set; }
        public string HeaderName { get; set; }
        public bool IsAutoValidate { get; set; }
    }
}
