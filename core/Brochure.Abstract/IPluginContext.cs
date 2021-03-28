using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Brochure.Abstract
{
    // public interface IPluginContextDescript { }

    /// <summary>
    /// The plugin context.
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// Gets the plugin context.
        /// </summary>
        /// <returns>A T.</returns>
       // T GetPluginContext<T>();

        IServiceCollection Services { get; }

    }
}