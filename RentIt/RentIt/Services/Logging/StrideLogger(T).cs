using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Services.Logging
{
    /// <summary>
    /// A Logging Service that logs messages to a Stride Room
    /// </summary>
    /// <typeparam name="T">The Category to log messages under</typeparam>
    public class StrideLogger<T> : ILogger<T>
    {
        private readonly StrideLogger _logger;

        /// <summary>
        /// Initializes a new instance of the StrideLogger class
        /// </summary>
        /// <param name="config">The Site Configuration object injected via DI</param>
        public StrideLogger(IConfiguration config)
        {
            _logger = new StrideLogger(config)
            {
                Category = typeof(T).FullName
            };
        }

        /// <summary>
        /// (Not Supported) - Adds a Scope 'Tag' to the log messages logged within the scope
        /// </summary>
        /// <typeparam name="TState">The Type of object the State 'Tag' is</typeparam>
        /// <param name="state">The Tag to use for the Scope</param>
        /// <returns>Returns null as this feature is unsupported</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        /// <summary>
        /// Checks to see if the given log level is enabled on this logger
        /// </summary>
        /// <param name="logLevel">The log level to check</param>
        /// <returns>Returns true if the log level is enabled</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        /// <summary>
        /// Logs a message to the configured Stride Room
        /// </summary>
        /// <typeparam name="TState">The Type of object the State parameter is</typeparam>
        /// <param name="logLevel">The log level the message should be logged at</param>
        /// <param name="eventId">The Event Id</param>
        /// <param name="state">The state to use when generating the message</param>
        /// <param name="exception">The exception to use when generating the message</param>
        /// <param name="formatter">A custom formatter to use for formatting the message</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
