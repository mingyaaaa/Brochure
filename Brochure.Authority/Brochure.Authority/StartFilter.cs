using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Abstract.Extensions;
using Brochure.Server.Main.Abstract.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Authority
{
    public class StartConfig : IStarupConfigure
    {
        public void Configure (Guid guid, IApplicationBuilder app)
        {
            // app.UseIdentityServer ();
            var log = app.ApplicationServices.GetService<ILogger<AuthorityPlugin>> ();

            //   app.IntertMiddle (guid, 100, () => app.UseAuthentication ());
            app.IntertMiddle (guid, 101, () =>
            {
                app.Use (t =>
                {
                    return async h =>
                    {
                        log.LogInformation ("3");
                        await t (h);
                        return;
                    };
                });
            });
            app.AddMiddle (guid, () =>
            {
                app.Use (t =>
                {
                    return async h =>
                    {
                        log.LogInformation ("4");
                        await t (h);
                    };
                });
            });
        }
    }
}