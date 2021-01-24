using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class DataFormatOptions: IOptions
    {
        public bool IsEnabled { get; set; } = false;
        public string RequestProvider { get; set; }
        public string ResponseProvider { get; set; }
    }
}
