using log4net;
using log4net.Repository;
using System.IO;
using System.Xml;

namespace ModelBase.Base.Logger
{
    /// <summary>
    /// Log4NetLogFactory
    /// </summary>
    public class Log4NetLogFactory : LogFactoryBase
    {

        //log4net日志
        private static readonly ILoggerRepository Repository = LogManager.CreateRepository("NETCoreRepository");
        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogFactory"/> class.
        /// </summary>
        public Log4NetLogFactory()
            : this("log4net.config")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogFactory"/> class.
        /// </summary>
        /// <param name="log4NetConfig">The log4net config.</param>
        public Log4NetLogFactory(string log4NetConfig)
            : base(log4NetConfig)
        {
            //log4net
            if (!IsSharedConfig)
            {
                log4net.Config.XmlConfigurator.Configure(Repository, new FileInfo(ConfigFile));
            }
            else
            {
                //Disable Performance logger
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                var docElement = xmlDoc.DocumentElement;
                var perfLogNode = docElement.SelectSingleNode("logger[@name='Performance']");
                if (perfLogNode != null)
                {
                    docElement.RemoveChild(perfLogNode);
                }

                log4net.Config.XmlConfigurator.Configure(Repository, docElement);
            }
        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override ILog GetLog(string name)
        {
            return new Log4NetLog(LogManager.GetLogger(Repository.Name, name));
        }
    }
}
