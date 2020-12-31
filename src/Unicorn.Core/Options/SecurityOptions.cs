using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class SecurityOptions
    {
        public SecurityOptions()
        {
            IPAllowedList = new List<string>();
            IPBlockedList = new List<string>();
        }

        public List<string> IPAllowedList { get; set; }

        public List<string> IPBlockedList { get; set; }

        public string EncryptProvider { get; set; }

        public string SignProvider { get; set; }
    }
}
