using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Extensions;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornABListMiddleware : UnicornMiddlewareBase<ABListOptions>
    {
        public UnicornABListMiddleware(UnicornContext context)
            :base(context.RouteRule.ABListOptions, context)
        {
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled == true)
            {
                if (!HandleABList(context, Options))
                {
                    UnicornContext.ResponseData = new ResponseData
                    {
                        StatusCode = Options.BlockedStatusCode,
                        StatusMessage = Options.BlockedMessage,
                    };
                    return;
                }
            }
            await next(context);
        }

        protected virtual bool HandleABList(HttpContext context, ABListOptions ablistOptions)
        {
            if (ablistOptions.IsAllowedEnabled())
            {
                return HandleAllowedList(context, ablistOptions);
            }
            else
            {
                return !HandleBlockedList(context, ablistOptions);
            }
        }

        protected virtual bool HandleAllowedList(HttpContext context, ABListOptions ablistOptions)
        {
            return HandleIPAllowedList(context, ablistOptions)
                || HandleUserAgentAllowedList(context, ablistOptions);
        }

        protected virtual bool HandleIPAllowedList(HttpContext context, ABListOptions ablistOptions)
        {
            return MatchIPs(context, ablistOptions.IPAllowedList);
        }

        protected virtual bool HandleUserAgentAllowedList(HttpContext context, ABListOptions ablistOptions)
        {
            return MatchUserAgents(context, ablistOptions.UserAgentAllowedList);
        }

        protected virtual bool HandleBlockedList(HttpContext context, ABListOptions ablistOptions)
        {
            return HandleIPBlockedList(context, ablistOptions)
                || HandleUserAgentBlockedList(context, ablistOptions);
        }

        protected virtual bool HandleIPBlockedList(HttpContext context, ABListOptions ablistOptions)
        {
            return MatchIPs(context, ablistOptions.IPBlockedList);
        }

        protected virtual bool HandleUserAgentBlockedList(HttpContext context, ABListOptions ablistOptions)
        {
            return MatchUserAgents(context, ablistOptions.UserAgentBlockedList);
        }

        protected virtual bool MatchIPs(HttpContext context, StringValues ips)
        {
            var userIp = context.GetClientIp().Split(',').FirstOrDefault()?.Trim();
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

        protected virtual bool MatchUserAgents(HttpContext context, StringValues userAgents)
        {
            var ua = context.Request.Headers["User-Agent"].ToString();
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
