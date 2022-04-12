using System.Reflection;
using System.Runtime.Loader;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin assembly load context.
    /// </summary>
    public class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly IAssemblyDependencyResolverProxy _resolver;
        private static IEnumerable<string> loadedAssemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginAssemblyLoadContext"/> class.
        /// </summary>
        static PluginAssemblyLoadContext()
        {
            loadedAssemblies = Default.Assemblies.Select(t => t.GetName().Name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginAssemblyLoadContext"/> class.
        /// </summary>
        /// <param name="resolverProxy">The resolver proxy.</param>
        public PluginAssemblyLoadContext(IAssemblyDependencyResolverProxy resolverProxy) : base(isCollectible: true)
        {
            _resolver = resolverProxy;
            this.Unloading += PluginsLoadContext_Unloading;
        }

        /// <summary>
        /// Plugins the load context_ unloading.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void PluginsLoadContext_Unloading(AssemblyLoadContext obj)
        {
            Console.WriteLine(obj.ToString());
        }

        /// <summary>
        /// Loads the.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>An Assembly.</returns>
        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (!string.IsNullOrWhiteSpace(assemblyPath) && !loadedAssemblies.Contains(assemblyName.Name))
            {
                try
                {
                    var ass = LoadFromAssemblyPath(assemblyPath);
                    return ass;
                }
                catch
                {
                }
            }
            return null;
        }

        /// <summary>
        /// Loads the unmanaged dll.
        /// </summary>
        /// <param name="unmanagedDllName">The unmanaged dll name.</param>
        /// <returns>An IntPtr.</returns>
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }
    }
}