using System;
using Brochure.Core.Models;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Server.Main.Extensions
{
    internal static class PluginOptionExtensions
    {
        internal static void AddController(this PluginOption pluginOption, IMvcBuilder mvcBuilder)
        {
            var assembly = pluginOption.GetAssembly();
            mvcBuilder.AddApplicationPart(assembly);
        }

        internal static void UseConfigure(this PluginOption pluginOption, IApplicationBuilder applicationBuilder)
        {
            var assembly = pluginOption.GetAssembly();
            var reflectorUtil = applicationBuilder.ApplicationServices.GetService<IReflectorUtil>();
            var configures = reflectorUtil.GetObjectByInterface<IStarupConfigure>(assembly);
            foreach (var item in configures)
            {
                item.Configure(applicationBuilder);
            }
        }
    }
}
