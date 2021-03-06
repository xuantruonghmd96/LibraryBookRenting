using Hangfire;
using LibraryBookRenting.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace LibraryBookRenting.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }

        public static void SetupBackgroundJob(this IApplicationBuilder app)
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            RecurringJob.AddOrUpdate<IUserService>(nameof(IUserService.SubstractCreditsEveryDay), x => x.SubstractCreditsEveryDay(), Cron.Daily(), TimeZoneInfo.Local);
        }
    }
}
