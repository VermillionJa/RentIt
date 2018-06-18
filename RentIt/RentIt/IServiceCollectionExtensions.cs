using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RentIt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt
{
    /// <summary>
    /// A collection of extension methods for the IServiceCollection interface
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Stride Logging framework to the Services Collection
        /// </summary>
        /// <param name="services">The Services Collection to add the Stride Logging to</param>
        public static void AddStrideLogging(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, StrideLogger>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new StrideLoggerProvider(serviceProvider));
        }
    }
}
