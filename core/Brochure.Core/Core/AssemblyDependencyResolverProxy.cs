using Brochure.Abstract;
using System.Reflection;
using System.Runtime.Loader;

namespace Brochure.Core
{
    /// <summary>
    /// The assembly dependency resolver proxy.
    /// </summary>
    public class AssemblyDependencyResolverProxy : IAssemblyDependencyResolverProxy
    {
        private readonly AssemblyDependencyResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDependencyResolverProxy"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public AssemblyDependencyResolverProxy(AssemblyDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDependencyResolverProxy"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public AssemblyDependencyResolverProxy(string path)
        {
            this.resolver = new AssemblyDependencyResolver(path);
        }

        /// <summary>
        /// Resolves the assembly to path.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>A string.</returns>
        public string ResolveAssemblyToPath(AssemblyName assemblyName)
        {
            return this.resolver.ResolveAssemblyToPath(assemblyName);
        }

        /// <summary>
        /// Resolves the unmanaged dll to path.
        /// </summary>
        /// <param name="unmanagedDllName">The unmanaged dll name.</param>
        /// <returns>A string.</returns>
        public string ResolveUnmanagedDllToPath(string unmanagedDllName)
        {
            return this.resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        }
    }
}