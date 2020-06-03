using System;
using System.Text;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Server.Main.Core;
using Brochure.Server.Main.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
            services.AddMvcCore ();
            services.AddLogging (t => t.AddConsole ());
            services.AddSingleton<IActionDescriptorChangeProvider> (PluginActionDescriptorChangeProvider.Instance);
            services.AddBrochureService (option =>
            {
                option.Services.AddSingleton<IBApplication> (new BApplication ());
                option.Services.TryAddSingleton<IMiddleManager> (new MiddleManager ());
                option.Services.AddTransient<IPluginUnLoadAction, PluginMiddleUnLoadAction> ();
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
            var routeOption = app.ApplicationServices.GetService<IOptions<RouteOptions>> ();
            routeOption.Value.SuppressCheckForUnhandledSecurityMetadata = true;
            if (application is BApplication t)
            {
                t.ServiceProvider = app.ApplicationServices;
                t.Builder = app;
            }

            app.IntertMiddle (Guid.Empty, 10, () => app.UseRouting ());
            app.ConfigPlugin ();
            app.IntertMiddle (Guid.Empty, 1000, () => app.UseEndpoints (endpoints => endpoints.MapControllers ()));
        }
    }
}