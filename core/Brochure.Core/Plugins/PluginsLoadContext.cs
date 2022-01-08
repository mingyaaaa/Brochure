using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using AspectCore.DynamicProxy.Parameters;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    /// <summary>
    /// The plugins load context.
    /// </summary>
    public class PluginsLoadContext : AssemblyLoadContext
    {
        private readonly IAssemblyDependencyResolverProxy _resolver;
        private static IEnumerable<string> loadedAssemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsLoadContext"/> class.
        /// </summary>
        static PluginsLoadContext()
        {
            loadedAssemblies = Default.Assemblies.Select(t => t.GetName().Name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsLoadContext"/> class.
        /// </summary>
        /// <param name="resolverProxy">The resolver proxy.</param>
        public PluginsLoadContext(IAssemblyDependencyResolverProxy resolverProxy) : base(isCollectible: true)
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
                    //   var ass = LoadFromStream(new FileStream(assemblyPath, FileMode.Open, FileAccess.Read));
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

        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>An Assembly.</returns>
        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            return LoadFromAssemblyName(assemblyName);
        }

        /// <summary>
        /// Uns the load.
        /// </summary>
        public void UnLoad()
        {
            Unload();
        }
    }

    /// <summary>
    /// The plugin load context proxy.
    /// </summary>
    public class PluginLoadContextProxy : IPluginsLoadContext
    {
        private readonly PluginsLoadContext pluginsLoadContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadContextProxy"/> class.
        /// </summary>
        /// <param name="pluginsLoadContext">The plugins load context.</param>
        public PluginLoadContextProxy(PluginsLoadContext pluginsLoadContext)
        {
            this.pluginsLoadContext = pluginsLoadContext;
        }

        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>An Assembly.</returns>
        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            return pluginsLoadContext.LoadAssembly(assemblyName);
        }

        /// <summary>
        /// Uns the load.
        /// </summary>
        public void UnLoad()
        {
            pluginsLoadContext.UnLoad();
        }
    }
}