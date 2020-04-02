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
        private readonly IAssemblyDependencyResolverProxy _resolver;
        private readonly HashSet<string> _sysAssemblyNameList;

        public PluginsLoadContext (IServiceCollection services, IAssemblyDependencyResolverProxy resolverProxy)
        {
            this.ServiceContrainer = services;
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

    public interface IAssemblyDependencyResolverProxy
    {
        string ResolveAssemblyToPath (AssemblyName assemblyName);
        string ResolveUnmanagedDllToPath (string name);
    }
    public class AssemblyDependencyResolverProxy : IAssemblyDependencyResolverProxy
    {
        private readonly AssemblyDependencyResolver resolver;

        public AssemblyDependencyResolverProxy (AssemblyDependencyResolver resolver)
        {
            this.resolver = resolver;
        }
        public AssemblyDependencyResolverProxy (string path)
        {
            this.resolver = new AssemblyDependencyResolver (path);
        }

        public string ResolveAssemblyToPath (AssemblyName assemblyName)
        {
            return this.resolver.ResolveAssemblyToPath (assemblyName);
        }

        public string ResolveUnmanagedDllToPath (string unmanagedDllName)
        {
            return this.resolver.ResolveUnmanagedDllToPath (unmanagedDllName);
        }
    }
}