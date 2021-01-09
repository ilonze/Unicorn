using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class RateLimitOptions : IOptions
    {
        public List<string> IPAllowedList { get; set; } = new List<string>();

        public List<string> UserAgentAllowedList { get; set; } = new List<string>();

        /// <summary>
        /// Enables endpoint rate limiting based URL path and HTTP verb
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Rate limit period as in 1s, 1m, 1h
        /// </summary>
        public long Period { get; set; }

        public long PeriodTimespan { get; set; }

        /// <summary>
        /// Maximum number of requests that a client can make in a defined period
        /// </summary>
        public long Limit { get; set; }

        public int QuotaExceededStatusCode { get; set; } = 429;

        public string QuotaExceededMessage { get; set; } = "Gateway Quota Exceeded";
    }
}
