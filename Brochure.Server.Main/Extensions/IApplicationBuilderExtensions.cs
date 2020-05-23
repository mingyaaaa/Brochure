using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Models;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Server.Main.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void ConfigPlugin (this IApplicationBuilder app)
        {
            var managers = app.ApplicationServices.GetService<IPluginManagers> ();
            var reflectUtil = app.ApplicationServices.GetService<IReflectorUtil> ();
            var plugins = managers.GetPlugins ();
            foreach (var item in plugins)
            {
                var configs = reflectUtil.GetObjectOfBase<IStarupConfigure> (item.Assembly);
                foreach (var config in configs)
                {
                    config.Configure (app);
                }
            }
        }
    }

}