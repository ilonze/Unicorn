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
            UserAgentAllowedList = new List<string>();
            UserAgentBlockedList = new List<string>();
        }

        public List<string> IPAllowedList { get; set; }

        public List<string> IPBlockedList { get; set; }

        public List<string> UserAgentAllowedList { get; set; }

        public List<string> UserAgentBlockedList { get; set; }

        public int BlockedStatusCode { get; set; } = 429;

        public string EncryptProvider { get; set; }

        public string SignProvider { get; set; }

        public bool IsABListEnabled()
        {
            return IPAllowedList.Count > 0 || IPBlockedList.Count > 0 || UserAgentAllowedList.Count > 0 || UserAgentBlockedList.Count > 0;
        }

        public bool IsAllowedEnabled()
        {
            return IPAllowedList.Count > 0 || UserAgentAllowedList.Count > 0;
        }

        public bool IsBlockedEnabled()
        {
            return IPBlockedList.Count > 0 || UserAgentBlockedList.Count > 0;
        }
    }
}
