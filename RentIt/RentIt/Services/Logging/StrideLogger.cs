using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RentIt.Extensions;
using RentIt.Helpers;
using RentIt.Models.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentIt.Services.Logging
{
    /// <summary>
    /// A Logging Service that logs messages to a Stride Room
    /// </summary>
    public class StrideLogger : ILogger
    {
        /// <summary>
        /// The Category, or Class, that the log messages are being sent from
        /// </summary>
        public string Category { get; set; }

        private readonly LogLevel _logLevel;
        private readonly string _strideUrl;
        private readonly string _strideAccessToken;

        /// <summary>
        /// Initializes a new instance of the StrideLogger class
        /// </summary>
        /// <param name="config">The Site Configuration object injected via DI</param>
        public StrideLogger(IConfiguration config)
        {
            _logLevel = Enum.Parse<LogLevel>(config["Logging:Stride:LogLevel:Default"], true);

            var siteId = config["Logging:Stride:Ids:Sites:CodeVermillion"];
            var roomId = config["Logging:Stride:Ids:Rooms:RentItLoggingJv"];

            _strideUrl = $"https://api.atlassian.com/site/{siteId}/conversation/{roomId}/message";

            _strideAccessToken = config["Logging:Stride:Tokens:RentIt_JV_SendMessage"];
        }

        /// <summary>
        /// (Not Supported) - Adds a Scope 'Tag' to the log messages logged within the scope
        /// </summary>
        /// <typeparam name="TState">The Type of object the State 'Tag' is</typeparam>
        /// <param name="state">The Tag to use for the Scope</param>
        /// <returns>Returns null as this feature is unsupported</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Checks to see if the given log level is enabled on this logger
        /// </summary>
        /// <param name="logLevel">The log level to check</param>
        /// <returns>Returns true if the log level is enabled</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
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
            if (!IsEnabled(logLevel))
            {
                return;
            }
            
            var message = state.ToString();
            var details = new LogDetailCollection();

            var strideState = state as (string Message, LogDetailCollection Details)?;

            if (strideState.HasValue)
            {
                message = strideState.Value.Message;
                details = strideState.Value.Details;
            }

            var fullLogMessage = FormatMessage(logLevel, message, details, exception);
            
            SendMessageToStride(fullLogMessage);
        }

        private string FormatMessage(LogLevel logLevel, string message, LogDetailCollection details, Exception exception)
        {
            var fullLogMessage = new StringBuilder();

            fullLogMessage.AppendLine($"Level: {logLevel}");
            fullLogMessage.AppendLine($"Category: {Category}");
            fullLogMessage.AppendLine($"Message: {message}");

            if (details?.Any() == true)
            {
                fullLogMessage.AppendLine("Details:");

                foreach (var detail in details)
                {
                    fullLogMessage.AppendLine($"{Chars.Tab}{detail.Key}: {detail.Value}");
                }
            }

            if (exception != null)
            {
                fullLogMessage.AppendLine("Exception:");

                fullLogMessage.AppendLine($"{Chars.Tab}Exception Type: {exception.GetType()}");
                fullLogMessage.AppendLine($"{Chars.Tab}Exception Message: {exception.Message}");
                fullLogMessage.AppendLine($"{Chars.Tab}Exception Stack Trace: {exception.StackTrace}");
            }

            return fullLogMessage.ToString();
        }

        private void SendMessageToStride(string message)
        {
            var restClient = new RestClient(_strideUrl);
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", $"Bearer {_strideAccessToken}");
            request.AddJsonBody(new
            {
                body = new
                {
                    version = 1,
                    type = "doc",
                    content = new[]
                    {
                        new {
                            type = "paragraph",
                            content = new[]
                            {
                                new
                                {
                                    type = "text",
                                    text = message
                                }
                            }
                        }
                    }
                }
            });

            restClient.Execute(request);
        }
    }
}
