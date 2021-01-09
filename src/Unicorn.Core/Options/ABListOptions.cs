using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class ABListOptions: IOptions
    {
        public bool IsEnabled { get; set; } = false;

        public StringValues IPAllowedList { get; set; }

        public StringValues IPBlockedList { get; set; }

        public StringValues UserAgentAllowedList { get; set; }

        public StringValues UserAgentBlockedList { get; set; }

        public int BlockedStatusCode { get; set; } = 429;

        public string BlockedMessage { get; set; } = "Gateway Blocked";

        public bool IsAllowedEnabled()
        {
            return IsEnabled && (IPAllowedList.Count > 0 || UserAgentAllowedList.Count > 0);
        }

        public bool IsBlockedEnabled()
        {
            return IsEnabled && (IPBlockedList.Count > 0 || UserAgentBlockedList.Count > 0);
        }
    }
}
