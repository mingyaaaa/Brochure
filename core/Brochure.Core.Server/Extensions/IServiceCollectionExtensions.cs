using Brochure.Abstract;
using Brochure.Core.PluginsDI;
using Brochure.Core.Server.Core;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

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
        internal static Task AddPluginController(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            services.TryAddSingleton(mvcBuilder);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Adds the brochure server.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration"></param>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public static async Task AddBrochureServer(this IServiceCollection services, IConfiguration configuration = null, Action<ApplicationOption> action = null)
        {
            services.AddLogging(t => t.AddConsole());
            services.AddBrochureCore(option =>
           {
               option.AddLog();
               option.Services.TryAddSingleton<IMiddleManager>(new MiddleManager());
               option.Services.AddTransient<IPluginUnLoadAction, PluginMiddleUnLoadAction>();
               option.Services.Replace(ServiceDescriptor.Transient(typeof(IApplicationBuilderFactory), typeof(PluginApplicationBuilderFactory)));
               action?.Invoke(option);
           }, configuration);
            services.AddHostedService<PluginLoadService>();
            services.AddSingleton<IPluginServiceProvider, DefaultPluginServiceProvider>();
            services.Replace(ServiceDescriptor.Scoped<IControllerActivator>(t =>
            {
                var pluginManager = t.GetService<IPluginManagers>();
                return new PluginScopeControllerActivator(pluginManager, new ServiceBasedControllerActivator());
            }));
            await services.AddPluginController();
            services.Replace(ServiceDescriptor.Singleton<IActionDescriptorChangeProvider>(PluginActionDescriptorChangeProvider.Instance));
            services.AddSingleton<IStartupFilter, PluginStartupFilter>();
        }

        /// <summary>
        /// Adds the autofac plugin.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddAutofacPlugin(this IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IPluginServiceProvider, AutofacPluginServiceProvider>());
            return services;
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
        [Obsolete]
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