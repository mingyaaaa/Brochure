using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core.Core
{
    public class Log
    {
        internal static ILogger Logger { get; set; }
        internal static IServiceCollection Services { get; set; }
        private static ILoggerFactory factory;
        public Log()
        {
            factory = Services.GetServiceInstance<ILoggerFactory>();
        }

        public static void Info() { }
        public static void Error() { }
        public static void Warning() { }

        public static void Info<T>()
        {
            var logger = factory.CreateLogger<T>();
            logger.LogInformation("");
        }
        public static void Error<T>() { }
        public static void Warning<T>() { }
    }
}
