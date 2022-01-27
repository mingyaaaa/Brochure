using Brochure.Abstract;
using Microsoft.Extensions.Configuration;
using System;

namespace Brochure.Core.Models
{
    /// <summary>
    /// The plugin option.
    /// </summary>
    public class PluginOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginOption"/> class.
        /// </summary>
        /// <param name="plugins">The plugins.</param>
        /// <param name="configuration"></param>
        public PluginOption(IPlugins plugins, IConfiguration configuration)
        {
            this._plugin = plugins;
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the plugin.
        /// </summary>
        private IPlugins _plugin { get; }

        /// <summary>
        /// Gets the plugin key.
        /// </summary>
        public Guid PluginKey => _plugin.Key;

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public string PluginName => _plugin.Name;

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; set; }
    }
}