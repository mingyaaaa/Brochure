using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    /// <summary>
    /// The application option extensions.
    /// </summary>
    public static class ApplicationOptionExtensions
    {
        /// <summary>
        /// Adds the log.
        /// </summary>
        /// <param name="applicationOption">The application option.</param>
        public static void AddLog (this ApplicationOption applicationOption)
        {
            Log.Logger = applicationOption.Services.GetServiceInstance<ILoggerFactory> ()?.CreateLogger ("Brochure");
            Log.Services = applicationOption.Services;
        }
    }
}