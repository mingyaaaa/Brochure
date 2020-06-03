using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public class Log
    {
        internal static ILogger Logger { get; set; }
        internal static IServiceCollection Services { get; set; }
        private static ILoggerFactory factory;
        public Log ()
        {
            factory = Services.GetServiceInstance<ILoggerFactory> ();
        }

        public static void Info (string msg)
        {
            EnsureLogger ();
            Logger.LogInformation (msg);
        }
        public static void Error (string msg, Exception e = null)
        {
            EnsureLogger ();
            Logger.LogError (msg, e);
        }
        public static void Warning (string msg)
        {
            EnsureLogger ();
            Logger.LogWarning (msg);
        }

        public static void Info<T> (string msg)
        {
            var logger = factory.CreateLogger<T> ();
            logger.LogInformation (msg);
        }
        public static void Error<T> (string msg, Exception e = null)
        {
            var logger = factory.CreateLogger<T> ();
            logger.LogError (msg, e);
        }
        public static void Warning<T> (string msg)
        {
            var logger = factory.CreateLogger<T> ();
            logger.LogWarning (msg);
        }

        private static void EnsureLogger ()
        {
            if (Logger == null)
            {
                EnsureLoggerFactorty ();
                Logger = Services.GetServiceInstance<ILoggerFactory> ()?.CreateLogger ("Brochure");
            }
        }
        private static void EnsureLoggerFactorty ()
        {
            if (factory == null)
            {
                factory = Services.GetServiceInstance<ILoggerFactory> ();
            }
        }
    }
}