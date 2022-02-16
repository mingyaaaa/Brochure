using Brochure.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin configuration load.
    /// </summary>
    public interface IPluginConfigurationLoad
    {
        /// <summary>
        /// Mergers the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="pluginDir"></param>
        /// <param name="plugins"></param>
        /// <returns>An IConfiguration.</returns>
        IConfiguration GetPluginConfiguration(IPlugins plugins);
    }
}