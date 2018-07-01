using Microsoft.Extensions.Logging;
using RentIt.Models.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Extensions
{
    /// <summary>
    /// A collection of extension methods that can be used on the <see cref="ILogger"/> Interface
    /// </summary>
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Logs a Information level message to Stride
        /// </summary>
        /// <param name="logger">The Logger to log the message to</param>
        /// <param name="message">The Message to Log</param>
        /// <param name="compileDetails">Additional Details to log along with the Message</param>
        public static void LogStrideInformation(this ILogger logger, string message, Func<LogDetailCollection, LogDetailCollection> compileDetails = null)
        {
            logger.LogStrideInformation(message, null, compileDetails);
        }

        /// <summary>
        /// Logs a Information level message to Stride
        /// </summary>
        /// <param name="logger">The Logger to log the message to</param>
        /// <param name="message">The Message to Log</param>
        /// <param name="exception">The Exception to log along with the Message</param>
        /// <param name="compileDetails">Additional Details to log along with the Message</param>
        public static void LogStrideInformation(this ILogger logger, string message, Exception exception, Func<LogDetailCollection, LogDetailCollection> compileDetails = null)
        {
            logger.LogInternal(LogLevel.Information, message, exception, compileDetails);
        }

        /// <summary>
        /// Logs a Debug level message to Stride
        /// </summary>
        /// <param name="logger">The Logger to log the message to</param>
        /// <param name="message">The Message to Log</param>
        /// <param name="compileDetails">Additional Details to log along with the Message</param>
        public static void LogStrideDebug(this ILogger logger, string message, Func<LogDetailCollection, LogDetailCollection> compileDetails = null)
        {
            logger.LogStrideDebug(message, null, compileDetails);
        }

        /// <summary>
        /// Logs a Debug level message to Stride
        /// </summary>
        /// <param name="logger">The Logger to log the message to</param>
        /// <param name="message">The Message to Log</param>
        /// <param name="exception">The Exception to log along with the Message</param>
        /// <param name="compileDetails">Additional Details to log along with the Message</param>
        public static void LogStrideDebug(this ILogger logger, string message, Exception exception, Func<LogDetailCollection, LogDetailCollection> compileDetails = null)
        {
            logger.LogInternal(LogLevel.Debug, message, exception, compileDetails);
        }

        private static void LogInternal(this ILogger logger, LogLevel logLevel, string message, Exception exception, Func<LogDetailCollection, LogDetailCollection> compileDetails)
        {
            var details = compileDetails == null ? new LogDetailCollection() : compileDetails(new LogDetailCollection());

            logger.Log(logLevel, 0, (message, details), exception, (s, e) => $"{s} - {e}");
        }
    }
}