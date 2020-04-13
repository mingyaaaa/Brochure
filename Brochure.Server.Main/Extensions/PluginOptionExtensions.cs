using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Brochure.Server.Main.Extensions
{
    internal static class PluginOptionExtensions
    {
        internal static void UseConfigure (this IPluginOption pluginOption, IApplicationBuilder applicationBuilder)
        {
            var assembly = pluginOption.Plugin.Assembly;
            var reflectorUtil = applicationBuilder.ApplicationServices.GetService<IReflectorUtil> ();
            var configures = reflectorUtil.GetObjectOfAbsoluteBase<IStarupConfigure> (assembly);
            foreach (var item in configures)
            {
                item.Configure (applicationBuilder);
            }
        }
    }
}