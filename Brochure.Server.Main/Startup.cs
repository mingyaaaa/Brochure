using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Extensions;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllers();
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var pluginUtil = new PluginUtil();
            var jsonUtil = new JsonUtil();
            var reflectorUtil = new ReflectorUtil();
            var objectFactory = new Brochure.Abstract.ObjectFactory();
            var sysDirectory = new SysDirectory();
            var pluginManager = new PluginManagers();
            var plugins = services.LoadPlugins(pluginUtil, sysDirectory, jsonUtil, objectFactory, reflectorUtil, loggerFactory, pluginManager);
            services.ResolverPlugins(pluginManager, t =>
            {
                t.AddController(mvcBuilder);
                t.AddStarupConfigureServices(services, reflectorUtil);
            });
            services.AddSingleton<IPluginUtil>(pluginUtil);
            services.AddSingleton<IJsonUtil>(jsonUtil);
            services.AddSingleton<IReflectorUtil>(reflectorUtil);
            services.AddSingleton<IObjectFactory>(objectFactory);
            services.AddSingleton<ISysDirectory>(sysDirectory);
            services.AddSingleton<IPluginManagers>(pluginManager);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            await app.AddPlugins();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
           });
        }
    }
}