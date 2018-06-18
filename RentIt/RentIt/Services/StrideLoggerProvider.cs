using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Services
{
    /// <summary>
    /// A Service for creating instances of the StrideLogger Service
    /// </summary>
    public class StrideLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialize a new instance of the StrideLoggerProvider class
        /// </summary>
        /// <param name="serviceProvider">The Service Provider for creating new instances via the DI framework</param>
        public StrideLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates a new instance of the StrideLogger class
        /// </summary>
        /// <param name="categoryName">(Not Supported) - The category name</param>
        /// <returns>A new StrideLogger instance</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return _serviceProvider.GetRequiredService<ILogger>();
        }

        /// <summary>
        /// Disposes of any unmanaged resources
        /// </summary>
        public void Dispose()
        {
            //Nothing to dispose
        }
    }
}
