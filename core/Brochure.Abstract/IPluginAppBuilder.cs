using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin app builder.
    /// </summary>
    public interface IPluginAppBuilder
    {
        /// <summary>
        /// Gets the app service.
        /// </summary>
        public IServiceProvider AppService { get; }

        /// <summary>
        /// Gets the plugin service.
        /// </summary>
        public ILifetimeScope PluginService { get; }
    }
}