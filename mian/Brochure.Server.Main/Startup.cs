using System;
using Brochure.Core;
using Brochure.Core.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Brochure.Server.Main
{
    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Main", Version = "v1" });
            });
            services.AddControllers();
            await services.AddBrochureServer();
        }

        // 
        /// <summary>
        ///This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="env">The env.</param>
        /// <param name="application">The application.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBApplication application)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // 添加Swagger有关中间件
            app.IntertMiddle("swagger", Guid.Empty, 8, () => app.UseSwagger());
            app.IntertMiddle("swaggerUI", Guid.Empty, 8, () => app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Main v1");
            }));

            var log = app.ApplicationServices.GetService<ILogger<Startup>>();
            var routeOption = app.ApplicationServices.GetService<IOptions<RouteOptions>>();
            routeOption.Value.SuppressCheckForUnhandledSecurityMetadata = true;
            if (application is BApplication t)
            {
                t.ServiceProvider = app.ApplicationServices;
                t.Builder = app;
            }
            app.IntertMiddle("main-routing", Guid.Empty, 10, () => app.UseRouting());
            app.ConfigPlugin();
            app.IntertMiddle("main-endpoint", Guid.Empty, int.MaxValue, () => app.UseEndpoints(endpoints => endpoints.MapControllers()));
        }
    }
}