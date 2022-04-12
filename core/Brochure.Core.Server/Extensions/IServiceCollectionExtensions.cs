using Brochure.Abstract;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The i service collection extensions.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the plugin controller.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>A Task.</returns>
        internal static async Task AddPluginController(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            services.TryAddSingleton<IMvcBuilder>(mvcBuilder);
            var provider = services.BuildServiceProvider();
            var manager = provider.GetService<IPluginManagers>();
            var pluginList = manager.GetPlugins();
            foreach (var item in pluginList)
            {
                try
                {
                    await item.StartAsync();
                    mvcBuilder.AddApplicationPart(item.Assembly);
                    Log.Info($"{item.Name}加载成功");
                }
                catch (Exception e)
                {
                    Log.Error($"{item.Name}加载失败", e);
                    await item.ExitAsync();
                }
            }
        }

        /// <summary>
        /// Adds the brochure server.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public static async Task AddBrochureServer(this IServiceCollection services, Action<ApplicationOption> action = null)
        {
            services.AddLogging(t => t.AddConsole());
            services.AddBrochureCore(option =>
           {
               option.AddLog();
               option.Services.TryAddSingleton<IMiddleManager>(new MiddleManager());
               option.Services.AddTransient<IPluginUnLoadAction, PluginMiddleUnLoadAction>();
               services.AddSingleton<IActionDescriptorChangeProvider>(PluginActionDescriptorChangeProvider.Instance);
               option.Services.Replace(ServiceDescriptor.Transient(typeof(IApplicationBuilderFactory), typeof(PluginApplicationBuilderFactory)));
               action?.Invoke(option);
           });
            await services.AddPluginController();
        }

        /// <summary>
        /// Adds the plugin swagger gen.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="xmlDirPath">The xml dir path.</param>
        public static void AddPluginSwaggerGen(this IServiceCollection services, string name, string version, string xmlDirPath)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = name, Version = version });
                c.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = name
                });
                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });
                // 获取xml文件路径
                var xmlFiles = Directory.GetFiles(xmlDirPath, "*.xml");
                foreach (var item in xmlFiles)
                {
                    // 添加控制器层注释，true表示显示控制器注释
                    c.IncludeXmlComments(item, true);
                }
            });
            services.Configure<Knife4UIOptions>(c =>
            {
                c.RoutePrefix = ""; // serve the UI at root
                c.SwaggerEndpoint($"/{version}/api-docs", name);
            });
        }

        /// <summary>
        /// Configures the plugin swagger gen.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="xmlDirPath">The xml dir path.</param>
        public static void ConfigurePluginSwaggerGen(this IServiceCollection services, string name, string version, string xmlDirPath)
        {
            services.ConfigureSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = name, Version = version });
                c.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = name
                });
                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });
                // 获取xml文件路径
                var xmlFiles = Directory.GetFiles(xmlDirPath, "*.xml");
                foreach (var item in xmlFiles)
                {
                    // 添加控制器层注释，true表示显示控制器注释
                    c.IncludeXmlComments(item, true);
                }
            });
            services.Configure<Knife4UIOptions>(c =>
            {
                c.RoutePrefix = ""; // serve the UI at root
                c.SwaggerEndpoint($"/{version}/api-docs", name);
            });
        }
    }
}