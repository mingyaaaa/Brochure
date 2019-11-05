using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.Core.Models;
using Brochure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddPlugins (this IServiceCollection service, IMvcBuilder mvcBuilder)
        {
            //处理插件
            service.AddTransient<IPluginManagers, PluginManagers> ();
            var pluginBathPath = PluginUtils.GetBasePluginsPath ();
            var allPluginPath = Directory.GetFiles (pluginBathPath, "plugin.config");
            var configBuilder = new ConfigurationBuilder ();
            var pluginTypeDic = new Dictionary<string, Tuple<ServiceDescriptor, Assembly>> ();
            foreach (var pluginPath in allPluginPath)
            {
                var pluginConfig = configBuilder.AddJsonFile (pluginPath).Build ().Get<PluginConfig> ();
                var locadContext = new PluginsLoadContext ();
                var assemably = locadContext.LoadFromAssemblyPath (Path.Combine (pluginBathPath, pluginConfig.Name, pluginConfig.AssemblyName));
                var allPluginTypes = ReflectorUtil.GetTypeByClass (assemably, typeof (Plugins));
                if (allPluginTypes.Count == 0)
                    throw new Exception ("请实现基于Plugins的插件类");
                if (allPluginTypes.Count == 2)
                    throw new Exception ("存在多个Plugins实现类");
                var pluginType = allPluginTypes[0];
                var serviceDescriptor = ServiceDescriptor.Singleton (typeof (Plugins), pluginType);
                service.Add (serviceDescriptor);
                pluginTypeDic.Add (pluginType.FullName, Tuple.Create (serviceDescriptor, assemably));
            }
            var provider = service.BuildServiceProvider ();
            var allPlugin = provider.GetServices<Plugins> ().ToList ();
            foreach (var item in allPlugin)
            {
                var tuple = pluginTypeDic[item.GetType ().FullName];
                if (item.Starting ())
                {
                    var assemably = tuple.Item2;
                    mvcBuilder.AddApplicationPart (assemably);
                }
                else
                {
                    service.Remove (tuple.Item1);
                }
            }
            return service;
        }
    }
}