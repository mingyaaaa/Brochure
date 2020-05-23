using System;
using System.Threading.Tasks;
using Brochure.Server.Main.Abstract.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Authority
{
    public class StartConfig : IStarupConfigure
    {
        public void Configure (IApplicationBuilder app)
        {
            // app.UseIdentityServer ();
            var middleManager = app.ApplicationServices.GetService<IMiddleManager> ();
            var log = app.ApplicationServices.GetService<ILogger<AuthorityPlugin>> ();
            middleManager.IntertMiddle (100, () => app.UseAuthentication ());
            middleManager.IntertMiddle (101, () =>
            {
                app.Use (t =>
                {
                    return h =>
                    {
                        log.LogInformation ("3");
                        t (h);
                        return Task.CompletedTask;
                    };
                });
            });
            middleManager.AddMiddle (() =>
            {
                app.Use (t =>
                {
                    return h =>
                    {
                        log.LogInformation ("4");
                        t (h);
                        return Task.CompletedTask;
                    };
                });
            });
        }
    }
}