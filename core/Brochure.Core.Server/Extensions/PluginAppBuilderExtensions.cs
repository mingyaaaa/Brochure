using Brochure.Abstract;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Extensions
{
    /// <summary>
    /// The plugin app builder extensions.
    /// </summary>
    internal static class PluginAppBuilderExtensions
    {
        /// <summary>
        /// Configures the plugin swagger gen.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="xmlDirPath">The xml dir path.</param>
        public static void ConfigurePluginSwaggerGen(this IPluginAppBuilder services, string name, string version, string xmlDirPath)
        {
            var swaggerOption = services.AppService.GetService<IOptions<SwaggerGenOptions>>();
            if (swaggerOption == null)
                return;
            var option = swaggerOption.Value;
            option.SwaggerDoc(version, new OpenApiInfo { Title = name, Version = version });
            option.AddServer(new OpenApiServer()
            {
                Url = "",
                Description = name
            });
            option.CustomOperationIds(apiDesc =>
            {
                var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                return controllerAction.ControllerName + "-" + controllerAction.ActionName;
            });
            // 获取xml文件路径
            var xmlFiles = Directory.GetFiles(xmlDirPath, "*.xml");
            foreach (var item in xmlFiles)
            {
                // 添加控制器层注释，true表示显示控制器注释
                option.IncludeXmlComments(item, true);
            }
            var knifeUiOption = services.AppService.GetService<IOptions<Knife4UIOptions>>();
            var uiOption = knifeUiOption.Value;
            uiOption.RoutePrefix = ""; // serve the UI at root
            uiOption.SwaggerEndpoint($"/{version}/api-docs", name);
        }
    }
}