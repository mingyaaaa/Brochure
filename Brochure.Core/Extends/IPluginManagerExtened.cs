using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Brochure.Core
{
    public static class IPluginManagerExtened
    {
        public static Assembly LoadPlugin (this IPluginManagers pluginManager, string configPath)
        {
            var pluginRecord = JsonUtil.ReadJson (configPath);
            //此对象是根据配置文件生成的 Plugin对象 没有具体的Run方法
            var plugin = pluginRecord.As<IPlugins> ();
            var pluginPath = Path.Combine (PluginManagers.GetPluginPath (), plugin.AssemblyName);
            if (!pluginPath.Contains (".dll"))
                pluginPath = $"{pluginPath}.dll";
            var stream = new FileStream (pluginPath, FileMode.Open, FileAccess.Read);
            var loadcontext = new LoadContext ();
            return loadcontext.LoadFromStream (stream);
        }

        public static IPlugins GetPlugin (this IPluginManagers pluginManager, string configPath)
        {
            var assembly = pluginManager.LoadPlugin (configPath);
            var loadcontext = AssemblyLoadContext.GetLoadContext (assembly);
            var plugins = ReflectorUtil.GetObjectByClass (assembly, typeof (Plugins));
            if (plugins.Count > 0)
                throw new Exception ($"该插件{assembly.FullName}有多个类继承Plugins方法");
            else if (plugins.Count == 0)
                throw new Exception ($"该插件{assembly.FullName}有没有类继承Plugins方法");
            var plugin = (IPlugins) plugins.First ();
            pluginManager.Regist (new PluginProxy (plugin, loadcontext));
            return plugin;
        }

        public static Assembly LoadPlugin (this IPluginManagers pluginManager, IPlugins plugin)
        {
            var pluginPath = Path.Combine (PluginManagers.GetPluginPath (), plugin.AssemblyName);
            return AssemblyLoadContext.Default.LoadFromAssemblyPath (pluginPath);
        }
    }
}