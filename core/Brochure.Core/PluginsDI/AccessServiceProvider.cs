using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The access service provider.
    /// </summary>
    public static class AccessServiceProvider
    {
        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        public static IServiceProvider Service { get; internal set; }
    }
}