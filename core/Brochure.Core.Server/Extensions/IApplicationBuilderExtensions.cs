using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The i application builder extensions.
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Configs the plugin.
        /// </summary>
        /// <param name="app">The app.</param>
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
                    foreach (var config in configs)
                    {
                        config.Configure(item.Key, app);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the middle.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="middleware">The middleware.</param>
        public static void AddMiddle(this IApplicationBuilder application, string middleName, Guid pluginId, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager>();
            middle.AddMiddle(middleName, pluginId, middleware);
        }

        /// <summary>
        /// Interts the middle.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="index">The index.</param>
        /// <param name="middleware">The middleware.</param>
        public static void IntertMiddle(this IApplicationBuilder application, string middleName, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager>();
            middle.IntertMiddle(middleName, pluginId, index, middleware);
        }

        /// <summary>
        /// Adds the middle.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="action">The action.</param>
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

        /// <summary>
        /// Interts the middle.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="index">The index.</param>
        /// <param name="action">The action.</param>
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