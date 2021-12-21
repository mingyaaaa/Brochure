using System;
using System.IO;
using System.Reflection;
using Brochure.Core.Server;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("main_v1", new OpenApiInfo { Title = "Main", Version = "main_v1" });
                c.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = "Main"
                });
                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });
                // 获取xml文件路径
                var xmlPath = AppContext.BaseDirectory;
                var xmlFiles = Directory.GetFiles(xmlPath, "*.xml");
                foreach (var item in xmlFiles)
                {
                    // 添加控制器层注释，true表示显示控制器注释
                    c.IncludeXmlComments(item, true);
                }
            });
            services.Configure<Knife4UIOptions>(c =>
            {
                c.RoutePrefix = ""; // serve the UI at root
                c.SwaggerEndpoint("/main_v1/api-docs", "Main");
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
            // 添加Swagger有关中间件
            app.IntertMiddle("swagger", Guid.Empty, 7, () => app.UseSwagger());
            app.IntertMiddle("swaggerUI", Guid.Empty, 8, () => app.UseKnife4UI());
            app.IntertMiddle("main-routing", Guid.Empty, 10, () => app.UseRouting());

            app.IntertMiddle("main-endpoint", Guid.Empty, int.MaxValue, () => app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("{documentName}/api-docs");
            }));
        }
    }
}