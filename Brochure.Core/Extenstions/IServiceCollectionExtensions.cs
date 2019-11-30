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
namespace Brochure.Core
{
    public static class IServiceCollectionExtensions
    {
        public static IEnumerable<ServiceDescriptor> AddPluginsServiceDescriptor (this IServiceCollection service, IPluginUtil pluginUtil)
        {
            //处理插件
            service.AddTransient<IPluginManagers, PluginManagers> ();
            var pluginBathPath = pluginUtil.GetBasePluginsPath ();
            var allPluginPath = Directory.GetFiles (pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList ();
            var configBuilder = new ConfigurationBuilder ();
            var plugins = new List<ServiceDescriptor> ();
            foreach (var pluginPath in allPluginPath)
            {
                var pluginConfig = configBuilder.AddJsonFile (pluginPath).Build ().Get<PluginConfig> ();
                var locadContext = new PluginsLoadContext ();
                //此处需要保证插件的文件夹的名称与 程序集的名称保持一致
                var assemably = locadContext.LoadFromAssemblyPath (Path.Combine (pluginBathPath, Path.GetFileNameWithoutExtension (pluginConfig.AssemblyName), pluginConfig.AssemblyName));
                var allPluginTypes = ReflectorUtil.GetTypeByClass (assemably, typeof (Plugins));
                if (allPluginTypes.Count == 0)
                    throw new Exception ("请实现基于Plugins的插件类");
                if (allPluginTypes.Count == 2)
                    throw new Exception ("存在多个Plugins实现类");
                var pluginType = allPluginTypes[0];
                var serviceDescriptor = ServiceDescriptor.Singleton<Plugins> (provider =>
                {
                    var constructor = pluginType.GetConstructor (new Type[] { typeof (PluginsLoadContext), typeof (IServiceProvider) });
                    return (Plugins) constructor.Invoke (new object[] { locadContext, provider });
                });
                plugins.Add (serviceDescriptor);
            }
            return plugins;
        }
    }
}