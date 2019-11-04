using System;
using System.Collections.Generic;
using System.IO;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Extensions;
using Brochure.Utils;
namespace Brochure.Core.Extenstions
{
    public static class IPluginManagerExtensions
    {
        public static IPlugins ResolvePlugins (this IPluginManagers pluginManager, string pluginConfigPath)
        {
            Plugins plugins = null;
            if (File.Exists (pluginConfigPath))
            {
                try
                {
                    var pluginRecord = JsonUtil.ReadJsonFile (pluginConfigPath);
                    //此对象是根据配置文件生成的 Plugin对象 没有具体的Run方法
                    plugins = new Plugins (new PluginsLoadContext ())
                    {
                        AssemblyName = pluginRecord[nameof (IPlugins.AssemblyName)].As<string> (),
                        Key = pluginRecord[nameof (IPlugins.Key)].As<Guid> (),
                        Author = pluginRecord[nameof (IPlugins.Author)].As<string> (),
                        DependencesKey = pluginRecord[nameof (IPlugins.DependencesKey)].As<List<Guid>> (),
                        Name = pluginRecord[nameof (IPlugins.Name)].As<string> (),
                        Version = pluginRecord[nameof (IPlugins.Version)].As<long> ()
                    };
                }
                catch (System.Exception e)
                {
                    throw new Exception ("插件配置文件异常", e);
                }
            }
            return plugins;
        }
    }
}