using System;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace ModelBase.Base.Logger
{

    public enum LogType
    {
        Console,
        Database
    }

    public static class Log
    {
        private static ILog _Logger = null;
        private static ILog m_Logger
        {
            get
            {
                if(_Logger == null)
                {
                    LogFactoryBase logbase = new Log4NetLogFactory();
                    _Logger = logbase.GetLog("ModelBase");
                }
                return _Logger;
            }
        }
        private static bool m_printConsole = true;        

        public static void Initialize(ILog logger,bool printConsole)
        {
            _Logger = logger;            
            m_printConsole = printConsole;
        }

        public static void Initialize()
        {
            var log = m_Logger;
            m_Logger.Info("Log Inited");
        }

        public static void Print(object message)
        {
            if (m_printConsole)
            {
                Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now, message));
            }
        }
        public static void Print(string format, params object[] args)
        {
            if (m_printConsole)
            {
                Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now, string.Format(format, args)));
            }
        }
        public static void Info(object message)
        {
            if (m_Logger != null && m_Logger.IsInfoEnabled)
            {
                m_Logger.Info(message);
            }
        }     
        public static void InfoFormat(string format, params object[] args)
        {
            if (m_Logger != null && m_Logger.IsInfoEnabled)
            {
                m_Logger.InfoFormat(format, args);
            }
        }
        public static void Error(object message)
        {
            if (m_Logger != null && m_Logger.IsErrorEnabled)
            {
                m_Logger.Error(message);
            }
        }
        public static void Error(object message, Exception exception)
        {
            if (m_Logger != null && m_Logger.IsErrorEnabled)
            {
                m_Logger.Error(message, exception);
            }
        }
        public static void ErrorFormat(string format, params object[] args)
        {
            if (m_Logger != null && m_Logger.IsErrorEnabled)
            {
                m_Logger.ErrorFormat(format, args);
            }
        }
        public static void Debug(object message)
        {
            if (m_Logger != null && m_Logger.IsDebugEnabled)
            {
                m_Logger.Debug(message);
            }
        }
        public static void DebugFormat(string format, params object[] args)
        {
            if (m_Logger != null && m_Logger.IsDebugEnabled)
            {
                m_Logger.DebugFormat(format, args);
            }
        }
        public static void Warn(object message)
        {
            if (m_Logger != null && m_Logger.IsWarnEnabled)
            {
                m_Logger.Warn(message);
            }
        }
        public static void WarnFormat(string format, params object[] args)
        {
            if (m_Logger != null && m_Logger.IsWarnEnabled)
            {
                m_Logger.WarnFormat(format, args);
            }
        }
        public static void Fatal(object message)
        {
            if (m_Logger != null && m_Logger.IsFatalEnabled)
            {
                m_Logger.Fatal(message);
            }
        }
        public static void Fatal(object message, Exception exception)
        {
            if (m_Logger != null && m_Logger.IsFatalEnabled)
            {
                m_Logger.Fatal(message, exception);
            }
        }
        public static void FatalFormat(string format, params object[] args)
        {
            if (m_Logger != null && m_Logger.IsFatalEnabled)
            {
                m_Logger.FatalFormat(format, args);
            }
        }
    }

    public class MyFileLog : ILog
    {
        private readonly object _syncRootConsole = new object();
        private readonly object _syncRootDataBase = new object();
        private readonly object _syncRootError = new object();
        private readonly string _rootDir;

        public MyFileLog(string rootDir)
        {
            _rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootDir);
            if (!Directory.Exists(_rootDir))
            {
                Directory.CreateDirectory(_rootDir);
            }
        }

        #region Debug
        /// <summary>
        /// database
        /// </summary>
        public void Debug(object message)
        {
        }

        public void Debug(object message, Exception exception)
        { }

        public void DebugFormat(string format, object arg0)
        { }

        public void DebugFormat(string format, params object[] args)
        { }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        { }

        public void DebugFormat(string format, object arg0, object arg1)
        { }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        { }
        #endregion

        #region Error
        public void Error(object message)
        { }

        public void Error(object message, Exception exception)
        { }

        public void ErrorFormat(string format, object arg0)
        { }

        public void ErrorFormat(string format, params object[] args)
        { }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        { }

        public void ErrorFormat(string format, object arg0, object arg1)
        { }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        { }
        #endregion

        #region Fatal
        public void Fatal(object message)
        { }

        public void Fatal(object message, Exception exception)
        { }

        public void FatalFormat(string format, object arg0)
        { }

        public void FatalFormat(string format, params object[] args)
        { }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        { }

        public void FatalFormat(string format, object arg0, object arg1)
        { }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        { }
        #endregion

        #region Info
        public void Info(object message)
        {
        }

        public void Info(object message, Exception exception)
        { }

        public void InfoFormat(string format, object arg0)
        { }

        public void InfoFormat(string format, params object[] args)
        { }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        { }

        public void InfoFormat(string format, object arg0, object arg1)
        { }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        { }
        #endregion

        #region Warn
        public void Warn(object message)
        { }

        public void Warn(object message, Exception exception)
        { }

        public void WarnFormat(string format, object arg0)
        { }

        public void WarnFormat(string format, params object[] args)
        { }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        { }

        public void WarnFormat(string format, object arg0, object arg1)
        { }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        { }
        #endregion

        public bool IsDebugEnabled { get { return true; } }//database
        public bool IsErrorEnabled { get { return true; } }//error
        public bool IsInfoEnabled { get { return true; } }//console
        public bool IsFatalEnabled { get { return false; } }
        public bool IsWarnEnabled { get { return false; } }
    }
}
