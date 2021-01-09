using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Unicorn.Middlewares;
using Unicorn.Options;

namespace Unicorn.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUnicorn(this IApplicationBuilder app, Action<UnicornOptions> optionSetup = null)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<UnicornOptions>>();
            if (optionSetup != null)
            {
                optionSetup?.Invoke(options.Value);
            }

            app.UseWhen(context => context.Request.Path.Value.ToLower().StartsWith("/api/unicorn/"), app =>
            {
                app.UseRouting();
                app.UseEndpoints(builder =>
                {
                    builder.MapControllers();
                });
            });

            app.UseWhen(context => context.MatchRoute(), app =>
            {
                app.UseMiddleware<UnicornMiddleware>();
                app.UseMiddleware<UnicornAntiResubmitMiddleware>();
                app.UseMiddleware<UnicornRateLimitMiddleware>();
                app.UseMiddleware<UnicornABListMiddleware>();
                app.UseMiddleware<UnicornCacheMiddleware>();
                app.UseMiddleware<UnicornAuthenticationMiddleware>();
                app.UseMiddleware<UnicornEncryptMiddleware>();
                app.UseMiddleware<UnicornSignMiddleware>();
                app.UseMiddleware<UnicornQoSMiddleware>();
                app.UseMiddleware<UnicornDegradationMiddleware>();
                app.UseMiddleware<UnicornLoadBalanceMiddleware>();
                app.UseMiddleware<UnicornRequestMiddleware>();
                app.UseMiddleware<UnicornAggregateMiddleware>();
                app.UseMiddleware<UnicornResponseMiddleware>();
            });

            return app;
        }
    }
}
