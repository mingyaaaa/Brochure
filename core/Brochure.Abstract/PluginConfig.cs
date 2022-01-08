using System;
using System.Collections.Generic;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin config.
    /// </summary>
    public class PluginConfig
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Guid Key { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Gets or sets the assembly name.
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the plugin path.
        /// </summary>
        public string PluginPath { get; set; }

        /// <summary>
        /// Gets or sets the dependences key.
        /// </summary>
        public List<Guid> DependencesKey { get; set; }
    }
}