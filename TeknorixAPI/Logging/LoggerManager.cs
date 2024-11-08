﻿using log4net;
using log4net.Config;
using System.Reflection;
using System.Xml;

namespace TeknorixAPI.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
        public LoggerManager()
        {
            try
            {
                var log4netConfig = new XmlDocument();

                using (var fs = File.OpenRead("log4net.config"))
                {
                    log4netConfig.Load(fs);

                    var repo = LogManager.CreateRepository(
                            Assembly.GetEntryAssembly(),
                            typeof(log4net.Repository.Hierarchy.Hierarchy));

                    XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error", ex);
            }
        }

        public void LogInformation(string message)
        {
            _logger.Info(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }
    }



}
