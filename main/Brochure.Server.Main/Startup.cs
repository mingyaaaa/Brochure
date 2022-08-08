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
using Swashbuckle.AspNetCore.SwaggerGen;

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
            services.AddBrochureServer(Configuration).ConfigureAwait(false).GetAwaiter().GetResult();
            services.AddPluginSwaggerGen("Main", "main_v1", AppContext.BaseDirectory);
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, "plugins", "Brochure.User");
            //services.ConfigurePluginSwaggerGen("User", "user_v1", xmlPath);
            //services.Configure<Knife4UIOptions>(knife4UIOptions =>
            //{
            //    knife4UIOptions.RoutePrefix = ""; // serve the UI at root
            //    knife4UIOptions.SwaggerEndpoint($"/user_v1/api-docs", "User");
            //});
        }

        //
        /// <summary>
        ///This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="env">The env.</param>
        /// <param name="uiOptions"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<Knife4UIOptions> uiOptions)
        {
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, "plugins", "Brochure.User");
            //var option1 = app.ApplicationServices.GetService<IOptions<SwaggerGenOptions>>().Value;
            //var option2 = app.ApplicationServices.GetService<IOptions<Knife4UIOptions>>().Value;
            //ConfigPluginSwaggerGen(option1, option2, "User", "user_v1", xmlPath);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var log = app.ApplicationServices.GetService<ILogger<Startup>>();
            var routeOption = app.ApplicationServices.GetService<IOptions<RouteOptions>>();
            routeOption.Value.SuppressCheckForUnhandledSecurityMetadata = true;
            // 添加Swagger有关中间件
            app.IntertMiddle("swagger", Guid.Empty, 7, () => app.UseSwagger());
            app.IntertMiddle("swaggerUI", Guid.Empty, 8, () => app.UseKnife4UI(uiOptions.Value));
            app.IntertMiddle("main-routing", Guid.Empty, 10, () => app.UseRouting());
            app.IntertMiddle("main-endpoint", Guid.Empty, int.MaxValue, () => app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("{documentName}/api-docs");
            }));
        }

        //public void ConfigPluginSwaggerGen(SwaggerGenOptions options, Knife4UIOptions knife4UIOptions, string name, string version, string xmlPath)
        //{
        //    options.SwaggerDoc(version, new OpenApiInfo { Title = name, Version = version });
        //    options.AddServer(new OpenApiServer()
        //    {
        //        Url = "",
        //        Description = name
        //    });
        //    options.CustomOperationIds(apiDesc =>
        //    {
        //        var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
        //        return controllerAction.ControllerName + "-" + controllerAction.ActionName;
        //    });
        //    // 获取xml文件路径
        //    var xmlFiles = Directory.GetFiles(xmlPath, "*.xml");
        //    foreach (var item in xmlFiles)
        //    {
        //        // 添加控制器层注释，true表示显示控制器注释
        //        options.IncludeXmlComments(item, true);
        //    }
        //    knife4UIOptions.RoutePrefix = ""; // serve the UI at root
        //    knife4UIOptions.SwaggerEndpoint($"/{version}/api-docs", name);
        //}
    }
}