using Brochure.Abstract;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin load context provider.
    /// </summary>
    public interface IPluginLoadContextProvider
    {
        /// <summary>
        /// Creates the load context.
        /// </summary>
        /// <param name="pluginConfigPath">The plugin config path.</param>
        /// <returns>An IPluginsLoadContext.</returns>
        IPluginsLoadContext CreateLoadContext(string pluginConfigPath);
    }

    /// <summary>
    /// The plugin load context provider.
    /// </summary>
    internal class PluginLoadContextProvider : IPluginLoadContextProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadContextProvider"/> class.
        /// </summary>
        public PluginLoadContextProvider()
        {
        }

        /// <summary>
        /// Creates the load context.
        /// </summary>
        /// <param name="pluginPath">The plugin path.</param>
        /// <returns>An IPluginsLoadContext.</returns>
        public IPluginsLoadContext CreateLoadContext(string pluginPath)
        {
            var assemblyDependencyResolverProxy = new AssemblyDependencyResolverProxy(pluginPath);
            return new PluginsLoadContext(assemblyDependencyResolverProxy);
        }
    }
}