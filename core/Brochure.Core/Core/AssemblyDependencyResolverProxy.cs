using System;
using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;

namespace Brochure.Core
{
    public class AssemblyDependencyResolverProxy : IAssemblyDependencyResolverProxy
    {
        private readonly AssemblyDependencyResolver resolver;

        public AssemblyDependencyResolverProxy(AssemblyDependencyResolver resolver)
        {
            this.resolver = resolver;
        }
        public AssemblyDependencyResolverProxy(string path)
        {
            this.resolver = new AssemblyDependencyResolver(path);
        }

        public string ResolveAssemblyToPath(AssemblyName assemblyName)
        {
            return this.resolver.ResolveAssemblyToPath(assemblyName);
        }

        public string ResolveUnmanagedDllToPath(string unmanagedDllName)
        {
            return this.resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        }
    }
}
