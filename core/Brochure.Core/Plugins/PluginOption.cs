using Brochure.Abstract;

namespace Brochure.Core.Models
{
    /// <summary>
    /// The plugin option.
    /// </summary>
    public class PluginOption : IPluginOption
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginOption"/> class.
        /// </summary>
        /// <param name="plugins">The plugins.</param>
        public PluginOption (IPlugins plugins)
        {
            this.Plugin = plugins;
        }

        /// <summary>
        /// Gets the plugin.
        /// </summary>
        public IPlugins Plugin { get; }

    }
}