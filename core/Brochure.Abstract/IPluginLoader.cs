using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin loader.
    /// </summary>
    public interface IPluginLoader
    {
        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        ValueTask LoadPlugin();

        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IPlugins> LoadPlugin(string path);

        /// <summary>
        /// Uns the load.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<bool> UnLoad(Guid key);

        /// <summary>
        /// Uns the load all.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        ValueTask<bool> UnLoadAll();
    }
}