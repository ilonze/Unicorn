using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class CacheOptions
    {
        public bool IsCahce { get; set; }

        public int TtlSeconds { get; set; }

        public string Region { get; set; }
    }
}
