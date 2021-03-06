﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class LoadBalanceOptions : IOptions
    {
        public bool IsEnabled { get; set; } = false;
        public string Provider { get; set; }
        public int Expiry { get; set; }
    }
}
