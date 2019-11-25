using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.Core.Models;
using Brochure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Server.Main
{
    internal static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="service"></param>
        /// <param name="mvcBuilder"></param>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        internal static IServiceCollection AddPlugins (this IServiceCollection service, IMvcBuilder mvcBuilder, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger ("AddPlugins");
            //处理插件           
            var manager = new PluginManagers ();
            var pluginBathPath = PluginUtils.GetBasePluginsPath ();
            var allPlugin = PluginManagers.ResolvePlugins (pluginBathPath, service);
            foreach (var item in allPlugin)
            {
                var task = Task.Run (async () =>
                {
                    try
                    {
                        var r = await item.StartingAsync ();
                        if (r)
                        {
                            await item.StartAsync ();
                            mvcBuilder.AddApplicationPart (item.Assembly);
                            manager.Regist (item);
                        }
                        else
                        {
                            r = await item.ExitingAsync ();
                            if (r)
                                await item.ExitAsync ();
                            logger.LogError ($"{item.Name}插件加载失败");
                        }
                    }
                    catch (System.Exception e)
                    {
                        logger.LogError (e, $"{item.Name}插件加载失败");
                    }
                    return Task.CompletedTask;
                });
                task.ConfigureAwait (false).GetAwaiter ().GetResult ();
            }
            return service;
        }
    }
}