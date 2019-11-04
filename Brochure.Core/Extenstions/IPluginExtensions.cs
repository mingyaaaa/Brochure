using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;

namespace Brochure.Core.Extenstions
{
    public static class IPluginExtensions
    {
        public static AssemblyLoadContext GetLoadContext (this IPlugins plugin)
        {
            return (plugin as Plugins)?.GetAssemblyLoadContext ();
        }
    }
}