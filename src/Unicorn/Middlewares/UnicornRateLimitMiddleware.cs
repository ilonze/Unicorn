﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Middlewares
{
    public class UnicornRateLimitMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
