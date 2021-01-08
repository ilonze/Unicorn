using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class QoSOptions
    {
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// 阀值
        /// </summary>
        public int Threshold { get; set; }

        public int Duration { get; set; }

        public int Timeout { get; set; }
    }
}
