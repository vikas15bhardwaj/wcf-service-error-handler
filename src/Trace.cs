using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Utilities
{
    public enum TraceType
    {
        /// <summary>
        /// Very detailed logs, which may include high-volume information such as protocol payloads. This log level is typically only enabled during development 
        /// </summary>
        Trace,
        /// <summary>
        /// debugging information, less detailed than trace, typically not enabled in production environment.
        /// </summary>
        Debug,
        /// <summary>
        /// information messages, which are normally enabled in production environment
        /// </summary>
        Information,
        /// <summary>
        /// warning messages, typically for non-critical issues, which can be recovered or which are temporary failures
        /// </summary>
        Warning,
        /// <summary>
        /// error messages
        /// </summary>
        Error,
        /// <summary>
        /// very serious errors
        /// </summary>
        Fatal
    }

    /// <summary>
    /// Trace class provides functionality to write log messages to various destinations using configuration.
    /// Its a wrapper around Nlog
    /// </summary>
    public class Trace
    {
        private static Dictionary<string, NLog.Logger> _nloggers = new Dictionary<string, NLog.Logger>();

        private static NLog.Logger GetLogger(string loggerName)
        {
            if (!_nloggers.ContainsKey(loggerName))
                _nloggers.Add(loggerName,LogManager.GetLogger(loggerName));

            return _nloggers[loggerName];
        }

        /// <summary>
        /// Write 
        /// </summary>
        /// <param name="type">TraceType indicating the type of message, whether its information only etc </param>
        /// <param name="message">the message to log</param>
        public static void Write(TraceType type,string message)
        {

            NLog.Logger nlogger = GetLogger(type.ToString());
            if (nlogger == null)
                throw new Exception("please check your configuration file, probably no logger is defined. The allowed loggers name are: Debug, Trace, Information, Warning, Error, and Fatal.");

            switch (type)
            {
                case TraceType.Trace:
                    nlogger.Trace(message);
                    break;
                case TraceType.Debug:
                    nlogger.Debug(message);
                    break;
                case TraceType.Information:
                    nlogger.Info(message);
                    break;
                case TraceType.Warning:
                    nlogger.Warn(message);
                    break;
                case TraceType.Error:
                    nlogger.Error(message);
                    break;
                case TraceType.Fatal:
                    nlogger.Fatal(message);
                    break;
                
            }

        }
    }
}
