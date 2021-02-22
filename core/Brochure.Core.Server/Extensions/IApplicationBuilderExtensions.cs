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
        public static void ConfigPlugin(this IApplicationBuilder app)
        {
            var managers = app.ApplicationServices.GetService<IPluginManagers>();
            var reflectUtil = app.ApplicationServices.GetService<IReflectorUtil>();
            var plugins = managers.GetPlugins();
            foreach (var item in plugins)
            {
                if (item is Plugins pp)
                {
                    var configs = reflectUtil.GetObjectOfBase<IStarupConfigure>(item.Assembly);
                    var pluginMiddleContext = pp.Context.GetPluginContext<PluginMiddleContext>();
                    foreach (var config in configs)
                    {
                        config.Configure(item.Key, pluginMiddleContext);
                    }
                }
            }
        }
        public static void AddMiddle(this IApplicationBuilder application, string middleName, Guid pluginId, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager>();
            middle.AddMiddle(middleName, pluginId, middleware);
        }

        public static void IntertMiddle(this IApplicationBuilder application, string middleName, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager>();
            middle.IntertMiddle(middleName, pluginId, index, middleware);
        }

        public static void AddMiddle(this IApplicationBuilder application, string middleName, Guid pluginId, Action action)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager>();
            Action<Func<RequestDelegate, RequestDelegate>> middleDelegate = t =>
            {
                middle.AddMiddle(middleName, pluginId, t);
            };
            middle.MiddleAction += middleDelegate;
            action?.Invoke();
            middle.MiddleAction -= middleDelegate;
        }

        public static void IntertMiddle(this IApplicationBuilder application, string middleName, Guid pluginId, int index, Action action)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager>();
            Action<Func<RequestDelegate, RequestDelegate>> middleDelegate = t =>
            {
                middle.IntertMiddle(middleName, pluginId, index, t);
            };
            middle.MiddleAction += middleDelegate;
            action?.Invoke();
            middle.MiddleAction -= middleDelegate;
        }

    }

}