using LogServer.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Brochure.Core.Core
{
    public class DbLogger : ILogger, IDisposable
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logger = new RpcClient<ILogService.Client>(LogServer.ServiceKey.LogServiceKey);
            var msg = formatter(state, exception);
            var cancellationToken = new CancellationToken();
            var log = new Log(msg, DateTime.Now.ToString(), exception.StackTrace);
            if (logLevel == LogLevel.Error)
                logger.Client.ErrorAsync(log, cancellationToken);
            else if (logLevel == LogLevel.Information)
                logger.Client.InfoAsync(log, cancellationToken);
            else
                logger.Client.WarningAsync(log, cancellationToken);
        }
    }

    public class DbLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger();
        }

        public void Dispose()
        {
        }
    }

    public static class DbLogExtends
    {
        public static ILoggerFactory AddDbLog(this ILoggerFactory factory)
        {
            factory.AddProvider(new DbLoggerProvider());
            return factory;
        }
    }
}