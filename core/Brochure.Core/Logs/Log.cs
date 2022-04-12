using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Brochure.Core
{
    /// <summary>
    /// The log.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        internal static ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        internal static IServiceCollection Services { get; set; }

        private static ILoggerFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
        {
            factory = Services.GetServiceInstance<ILoggerFactory>();
        }

        /// <summary>
        /// Infos the.
        /// </summary>
        /// <param name="msg">The msg.</param>
        public static void Info(string msg)
        {
            EnsureLogger();
            Logger.LogInformation(msg);
        }

        /// <summary>
        /// Errors the.
        /// </summary>
        /// <param name="msg">The msg.</param>
        /// <param name="e">The e.</param>
        public static void Error(string msg, Exception e = null)
        {
            EnsureLogger();
            Logger.LogError(msg, e);
        }

        /// <summary>
        /// Warnings the.
        /// </summary>
        /// <param name="msg">The msg.</param>
        public static void Warning(string msg)
        {
            EnsureLogger();
            Logger.LogWarning(msg);
        }

        /// <summary>
        /// Infos the.
        /// </summary>
        /// <param name="msg">The msg.</param>
        public static void Info<T>(string msg)
        {
            var logger = factory.CreateLogger<T>();
            logger.LogInformation(msg);
        }

        /// <summary>
        /// Errors the.
        /// </summary>
        /// <param name="msg">The msg.</param>
        /// <param name="e">The e.</param>
        public static void Error<T>(string msg, Exception e = null)
        {
            var logger = factory.CreateLogger<T>();
            logger.LogError(msg, e);
        }

        /// <summary>
        /// Warnings the.
        /// </summary>
        /// <param name="msg">The msg.</param>
        public static void Warning<T>(string msg)
        {
            var logger = factory.CreateLogger<T>();
            logger.LogWarning(msg);
        }

        /// <summary>
        /// Ensures the logger.
        /// </summary>
        private static void EnsureLogger()
        {
            if (Logger == null)
            {
                EnsureLoggerFactorty();
                Logger = Services.GetServiceInstance<ILoggerFactory>()?.CreateLogger("Brochure");
            }
        }

        /// <summary>
        /// Ensures the logger factorty.
        /// </summary>
        private static void EnsureLoggerFactorty()
        {
            if (factory == null)
            {
                factory = Services.GetServiceInstance<ILoggerFactory>();
            }
        }
    }
}