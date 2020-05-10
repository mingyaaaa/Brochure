using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core
{
    public class PluginsLoadContext : AssemblyLoadContext
    {
        public IServiceProvider Service { get; }
        private readonly IAssemblyDependencyResolverProxy _resolver;
        private readonly HashSet<string> _sysAssemblyNameList;

        public PluginsLoadContext (IServiceProvider services, IAssemblyDependencyResolverProxy resolverProxy) : base (isCollectible: true)
        {
            this.Service = services;
            _resolver = resolverProxy;
            _sysAssemblyNameList = new HashSet<string> (Default.Assemblies.Select (t => t.GetName ().Name));
        }

        protected override Assembly Load (AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath (assemblyName);
            if (assemblyPath != null && !_sysAssemblyNameList.Contains (assemblyName.Name))
            {
                return LoadFromAssemblyPath (assemblyPath);
            }
            return null;
        }

        protected override IntPtr LoadUnmanagedDll (string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath (unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath (libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}