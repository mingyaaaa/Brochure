using Brochure.Abstract;
using Brochure.Core;
using Brochure.System;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        public void ConfigureServices (IServiceCollection services)
        {
            var mvcBuilder = services.AddControllers ();
            var loggerFactory = LoggerFactory.Create (_ => { });
            var pluginUtil = new PluginUtil ();
            var jsonUtil = new JsonUtil ();
            var reflectorUtil = new ReflectorUtil ();
            var objectFactory = new Abstract.ObjectFactory ();
            var sysDirectory = new SysDirectory ();
            var pluginManager = new PluginManagers ();
            var plugins = services.ResolvePlugins (pluginUtil, sysDirectory, jsonUtil, objectFactory, reflectorUtil);
            services.AddPlugins (mvcBuilder, loggerFactory, pluginManager, plugins);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints =>
            {
                endpoints.MapControllers ();
            });
        }
    }
}