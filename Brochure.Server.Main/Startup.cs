using System.Reflection.Emit;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Server.Main.Core;
using Brochure.Server.Main.Extensions;
using Brochure.SysInterface;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace Brochure.Server.Main
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices (IServiceCollection services)
        {
            services.AddLogging ();
            services.AddSingleton<IActionDescriptorChangeProvider> (PluginActionDescriptorChangeProvider.Instance);
            services.AddBrochureService (option =>
            {
                option.Services.AddSingleton<IBApplication> (new BApplication ());
                option.Services.AddSingleton<IMiddleManager, MiddleManager> ();
                option.Services.Replace (ServiceDescriptor.Transient (typeof (IApplicationBuilderFactory), typeof (PluginApplicationBuilderFactory)));
            });
            await services.AddPluginController ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IBApplication application)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }

            var log = app.ApplicationServices.GetService<ILogger<Startup>> ();
            var middleManager = app.ApplicationServices.GetService<IMiddleManager> ();
            if (application is BApplication t)
            {
                t.ServiceProvider = app.ApplicationServices;
                t.Builder = app;
            }
            middleManager.IntertMiddle (middleManager.GetMiddleCount () + 1, () => app.UseRouting ());

            middleManager.AddMiddle (() =>
            {
                app.Use (t =>
                {
                    return h =>
                    {
                        log.LogInformation ("1");
                        t (h);
                        return Task.CompletedTask;
                    };;
                });
            });
            middleManager.AddMiddle (() =>
            {
                app.Use (t =>
                {
                    return h =>
                    {
                        log.LogInformation ("2");
                        t (h);
                        return Task.CompletedTask;
                    };
                });
            });
            app.ConfigPlugin ();
            middleManager.IntertMiddle (1000, () => app.UseEndpoints (endpoints => endpoints.MapControllers ()));
        }
    }
}