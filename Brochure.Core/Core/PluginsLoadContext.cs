using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core
{
    public class PluginsLoadContext : AssemblyLoadContext
    {
        public IServiceCollection ServiceContrainer;
        private readonly AssemblyDependencyResolver _resolver;
        private HashSet<string> _sysAssemblyNameList;
        public PluginsLoadContext(IServiceCollection services, string path)
        {
            this.ServiceContrainer = services;
            _resolver = new AssemblyDependencyResolver(path);

            _sysAssemblyNameList = new HashSet<string>(Default.Assemblies.Select(t => t.GetName().Name));
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null && !_sysAssemblyNameList.Contains(assemblyName.Name))
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }

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