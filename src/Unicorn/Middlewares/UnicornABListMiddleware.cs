using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicorn.Caches;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornABListMiddleware : IMiddleware
    {
        protected UnicornOptions Options { get; }
        protected UnicornContext UnicornContext { get; }
        public UnicornABListMiddleware(
            IOptions<UnicornOptions> options,
            UnicornContext unicornContext)
        {
            Options = options.Value;
            UnicornContext = unicornContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (UnicornContext.RouteRule.SecurityOptions?.IsABListEnabled() == true)
            {
                if (!HandleABList(context, UnicornContext.RouteRule.SecurityOptions))
                {
                    UnicornContext.ResponseData = new ResponseData
                    {
                        StatusCode = Options.GlobalOptions.SecurityOptions.BlockedStatusCode,
                    };
                    return;
                }
            }
            else if(Options.GlobalOptions.SecurityOptions?.IsABListEnabled() == true)
            {
                if (!HandleABList(context, Options.GlobalOptions.SecurityOptions))
                {
                    UnicornContext.ResponseData = new ResponseData
                    {
                        StatusCode = Options.GlobalOptions.SecurityOptions.BlockedStatusCode,
                    };
                    return;
                }
            }
            await next(context);
        }

        protected virtual bool HandleABList(HttpContext context, SecurityOptions securityOptions)
        {
            if (securityOptions.IsAllowedEnabled())
            {
                return HandleAllowedList(context, securityOptions);
            }
            else
            {
                return !HandleBlockedList(context, securityOptions);
            }
        }

        protected virtual bool HandleAllowedList(HttpContext context, SecurityOptions securityOptions)
        {
            return HandleIPAllowedList(context, securityOptions)
                || HandleUserAgentAllowedList(context, securityOptions);
        }

        protected virtual bool HandleIPAllowedList(HttpContext context, SecurityOptions securityOptions)
        {
            return MatchIPs(context, securityOptions.IPAllowedList);
        }

        protected virtual bool HandleUserAgentAllowedList(HttpContext context, SecurityOptions securityOptions)
        {
            return MatchUserAgents(context, securityOptions.UserAgentAllowedList);
        }

        protected virtual bool HandleBlockedList(HttpContext context, SecurityOptions securityOptions)
        {
            return HandleIPBlockedList(context, securityOptions)
                || HandleUserAgentBlockedList(context, securityOptions);
        }

        protected virtual bool HandleIPBlockedList(HttpContext context, SecurityOptions securityOptions)
        {
            return MatchIPs(context, securityOptions.IPBlockedList);
        }

        protected virtual bool HandleUserAgentBlockedList(HttpContext context, SecurityOptions securityOptions)
        {
            return MatchUserAgents(context, securityOptions.UserAgentBlockedList);
        }

        protected virtual bool MatchIPs(HttpContext context, List<string> ips)
        {
            var userIp = context.Connection.RemoteIpAddress.ToString();
            if (userIp.Contains(":"))
            {
                foreach (var ip in ips)
                {
                    if (ip.EndsWith("::") || ip.EndsWith("::0"))
                    {
                        var _ip = ip.Split("::")[0] + ":";
                        if (userIp.StartsWith(_ip))
                        {
                            return true;
                        }
                    }
                    else if(ip == userIp)
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (var ip in ips)
                {
                    var _ip = ip;
                    while (_ip.EndsWith(".0"))
                    {
                        _ip = _ip[0..^2];
                    }
                    if (_ip.Count(r => r == '.') < 3)
                    {
                        _ip = _ip + ".";
                        if (userIp.StartsWith(_ip))
                        {
                            return true;
                        }
                    }
                    else if (_ip == userIp)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool MatchUserAgents(HttpContext context, List<string> userAgents)
        {
            var ua = context.Request.Headers["UserAgent"].ToString();
            foreach (var userAgent in userAgents)
            {
                try
                {
                    if (ua.Contains(userAgent) || new Regex(userAgent).IsMatch(ua))
                    {
                        return true;
                    }
                }
                catch
                {
                }
            }
            return false;
        }
    }
}
