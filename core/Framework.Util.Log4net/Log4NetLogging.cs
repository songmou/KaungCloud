using Framework.Core.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Util.Log4net
{
    public class Log4NetLogging : ILogger
    {
        private ILog logger;

        public Log4NetLogging(string repositoryName = "repository", string loggerName = "baselog")
        {
            //ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            //// 默认简单配置，输出至控制台
            //BasicConfigurator.Configure(repository);
            //ILog log = LogManager.GetLogger(repository.Name, "NETCorelog4net");

            log4net.Repository.ILoggerRepository repository = LogManager.CreateRepository(repositoryName);
            log4net.Config.XmlConfigurator.Configure(repository, new System.IO.FileInfo("Configs/log4net.config"));
            ILog log = LogManager.GetLogger(repository.Name, loggerName);
            this.logger = log;
        }

        public void Debug<T>(T message)
        {
            this.logger.Debug(message);
        }

        public void Error<T>(T message)
        {
            this.logger.Error(message);
        }

        public void Error<T>(T message, Exception exception)
        {
            this.logger.Error(message, exception);
        }

        public void Info<T>(T message)
        {
            this.logger.Info(message);
        }

        public void Warn<T>(T message)
        {
            this.logger.Warn(message);
        }
    }
}
