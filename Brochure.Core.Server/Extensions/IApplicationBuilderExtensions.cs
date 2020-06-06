using System;
using Brochure.Abstract;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Server
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
                if (item is Plugins pp)
                {
                    var configs = reflectUtil.GetObjectOfBase<IStarupConfigure> (item.Assembly);
                    var pluginMiddleManager = pp.Context.GetPluginContext<PluginMiddleContext> ();
                    foreach (var config in configs)
                    {
                        config.Configure (item.Key, pluginMiddleManager);
                    }
                }
            }
        }
        public static void AddMiddle (this IApplicationBuilder application, Guid pluginId, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.AddMiddle (pluginId, middleware);
        }

        public static void IntertMiddle (this IApplicationBuilder application, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.IntertMiddle (pluginId, index, middleware);
        }

        public static void AddMiddle (this IApplicationBuilder application, Guid pluginId, Action action)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.AddMiddle (pluginId, action);
        }

        public static void IntertMiddle (this IApplicationBuilder application, Guid pluginId, int index, Action action)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.IntertMiddle (pluginId, index, action);
        }

    }

}