using System;
using System.IO;
using System.Reflection;
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
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("main_v1", new OpenApiInfo { Title = "Main", Version = "main_v1" });

                // 获取xml文件名
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // 获取xml文件路径
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 添加控制器层注释，true表示显示控制器注释
                c.IncludeXmlComments(xmlPath, true);
            });
            services.Configure<SwaggerUIOptions>(t =>
            {
                t.SwaggerEndpoint("/swagger/main_v1/swagger.json", "Main");
            });
            services.AddBrochureServer().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        //
        /// <summary>
        ///This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="env">The env.</param>
        /// <param name="application">The application.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var log = app.ApplicationServices.GetService<ILogger<Startup>>();
            var routeOption = app.ApplicationServices.GetService<IOptions<RouteOptions>>();
            routeOption.Value.SuppressCheckForUnhandledSecurityMetadata = true;
            var bb = app.ApplicationServices.GetServices<IConfigureOptions<SwaggerUIOptions>>();
            var a = app.ApplicationServices.GetService<IOptionsSnapshot<SwaggerUIOptions>>().Value;
            // 添加Swagger有关中间件
            app.IntertMiddle("swagger", Guid.Empty, 7, () => app.UseSwagger());
            app.IntertMiddle("swaggerUI", Guid.Empty, 8, () => app.UseSwaggerUI());
            app.IntertMiddle("main-routing", Guid.Empty, 10, () => app.UseRouting());
            app.ConfigPlugin();

            app.IntertMiddle("main-endpoint", Guid.Empty, int.MaxValue, () => app.UseEndpoints(endpoints => endpoints.MapControllers()));
        }
    }
}