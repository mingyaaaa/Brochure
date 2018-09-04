using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Brochure.Core
{
    public static class IPluginManagerExtened
    {
        public static Assembly LoadPlugin(this IPluginManagers pluginManager, IPlugins plugin)
        {
            var pluginPath = Path.Combine(PluginManagers.GetPluginPath(), plugin.AssemblyName);
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(pluginPath);
        }
    }
}
