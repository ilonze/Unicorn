using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
                app.UseMiddleware<UnicornLoadBalanceMiddleware>();
                app.UseMiddleware<UnicornDataFormatMiddleware>();
                app.UseMiddleware<UnicornRequestMiddleware>();
                app.UseMiddleware<UnicornAggregateMiddleware>();
                app.UseMiddleware<UnicornResponseMiddleware>();
            });

            var timer = new Timer();
            var isChecking = false;
            timer.Elapsed += (o, e) =>
            {
                if (isChecking) return;
                isChecking = true;
                var options = app.ApplicationServices.GetRequiredService<IOptions<UnicornOptions>>().Value;
                var services = options.Services;
                var healthCheckChanges = new List<Service>();
                foreach (var service in services)
                {
                    if (HealthCheck(service, options.HealthCheckInterval))
                    {
                        healthCheckChanges.Add(service);
                    }
                }
                //TODO:广播通知其他网关
                isChecking = false;
            };
            timer.Interval = 1000;
            timer.Start();

            return app;
        }

        private static bool HealthCheck(Service service, int interval)
        {
            if (service.CheckTime.AddSeconds(interval) > DateTimeOffset.Now)
            {
                return false;
            }
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(2)
            };
            var message = client.GetAsync(service.HealthCheck).Result;
            var origin = service.IsHealth;
            service.IsHealth = message.IsSuccessStatusCode;
            service.CheckTime = DateTimeOffset.Now;
            return service.IsHealth != origin;
        }
    }
}
