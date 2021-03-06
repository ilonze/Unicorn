﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class EncryptOptions: IOptions
    {
        public bool IsEnabled { get; set; } = false;
        public string RequestProvider { get; set; }
        public string ResponseProvider { get; set; }
        public int InvalidStatusCode { get; set; } = 400;
        public string InvalidMessage { get; set; }
        public string EncryptKey { get; set; }
        public string DecryptKey { get; set; }
        public Dictionary<string, string> Extra { get; set; }
    }
}
