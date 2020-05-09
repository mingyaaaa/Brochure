using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Core;
using Brochure.Server.Main.Extensions;
using Brochure.SysInterface;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddBrochureService (option => option.Services.AddSingleton<IBApplication> (new BApplication () { Services = services }));
            await services.AddPluginController ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IBApplication application)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }
            //   app.UseHttpsRedirection ();
            (application as BApplication).ServiceProvider = app.ApplicationServices;
            app.UseRouting ();

            app.UseEndpoints (endpoints =>
            {
                endpoints.MapControllers ();
            });
        }
    }
}