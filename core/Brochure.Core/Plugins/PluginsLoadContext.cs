using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core
{
    /// <summary>
    /// The plugins load context.
    /// </summary>
    public class PluginsLoadContext : AssemblyLoadContext, IPluginsLoadContext
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        public IPluginServiceProvider Service { get; }
        private readonly IAssemblyDependencyResolverProxy _resolver;
        private readonly HashSet<string> _sysAssemblyNameList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsLoadContext"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="resolverProxy">The resolver proxy.</param>
        public PluginsLoadContext(IServiceProvider services, IAssemblyDependencyResolverProxy resolverProxy) : base(isCollectible: true)
        {
            this.Service = services as IPluginServiceProvider;
            if (this.Service == null)
                throw new Exception("请传入IPluginServiceProvider类型");
            _resolver = resolverProxy;
            _sysAssemblyNameList = new HashSet<string>(Default.Assemblies.Select(t => t.GetName().Name));
        }

        /// <summary>
        /// Loads the.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>An Assembly.</returns>
        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null && !_sysAssemblyNameList.Contains(assemblyName.Name))
            {
                var ass = LoadFromAssemblyPath(assemblyPath);
                _sysAssemblyNameList.Add(assemblyName.Name);
                return ass;
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
}