using Brochure.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Brochure.Core
{
    /// <summary>
    /// The plugins load context.
    /// </summary>
    public class PluginsLoadContext : AssemblyLoadContext, IPluginsLoadContext
    {
        private readonly IAssemblyDependencyResolverProxy _resolver;
        private static IEnumerable<string> loadedAssemblies;
        private static ConcurrentDictionary<string, AssemblyCount> ShareAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsLoadContext"/> class.
        /// </summary>
        static PluginsLoadContext()
        {
            loadedAssemblies = Default.Assemblies.Select(t => t.GetName().FullName);
            ShareAssembly = new ConcurrentDictionary<string, AssemblyCount>();
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
            if (!string.IsNullOrWhiteSpace(assemblyPath) && !loadedAssemblies.Contains(assemblyName.FullName))
            {
                try
                {
                    if (ShareAssembly.TryGetValue(assemblyName.FullName!, out var assemCount))
                    {
                        assemCount.Count++;
                        return assemCount.Assembly;
                    }
                    var ass = LoadFromAssemblyPath(assemblyPath);
                    ShareAssembly.TryAdd(assemblyName.FullName, new AssemblyCount(ass, 1));
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
            foreach (var item in Assemblies)
            {
                if (ShareAssembly.TryGetValue(item.FullName!, out var assemblyCount))
                {
                    assemblyCount.Count--;
                    if (assemblyCount.Count == 0)
                        ShareAssembly.TryRemove(item.FullName!, out _);
                }
            }
            Unload();
        }

        private class AssemblyCount
        {
            public AssemblyCount(Assembly assembly, int count)
            {
                Assembly = assembly;
                Count = count;
            }

            internal Assembly Assembly { get; }

            internal int Count { get; set; }
        }
    }
}