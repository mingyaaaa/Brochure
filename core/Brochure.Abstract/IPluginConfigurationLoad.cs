using Brochure.Abstract;
using Microsoft.Extensions.Configuration;

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
        /// <param name="plugins"></param>
        /// <returns>An IConfiguration.</returns>
        IConfiguration GetPluginConfiguration(IPlugins plugins);
    }
}