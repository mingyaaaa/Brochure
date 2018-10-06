using log4net;
using log4net.Config;
using System;
using System.IO;
using ILog = Brochure.Core.Interfaces.ILog;

namespace Brochure.Core.Server
{
    public class Log : ILog
    {
        private readonly log4net.ILog logger;
        public Log()
        {
            var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            logger = LogManager.GetLogger(repository.Name, "InfoLogger");
        }
        public void Error(string message, Exception e)
        {
            logger.Error(message, e);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Debug(string messgae)
        {
            logger.Debug(messgae);
        }
    }
}
