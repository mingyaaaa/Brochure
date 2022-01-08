using System.Reflection;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugins load context.
    /// </summary>
    public interface IPluginsLoadContext
    {
        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>An Assembly.</returns>
        Assembly LoadAssembly(AssemblyName assemblyName);

        /// <summary>
        /// Uns the load.
        /// </summary>
        void UnLoad();
    }
}