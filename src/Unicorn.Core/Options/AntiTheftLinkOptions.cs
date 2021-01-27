using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class AntiTheftLinkOptions: IOptions
    {
        public bool IsEnabled { get; set; } = false;

        public List<string> AllowHosts { get; set; }

        public int BlockedStatusCode { get; set; } = 429;

        public string BlockedMessage { get; set; } = "Gateway Blocked";
    }
}
