using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RentIt
{
    /// <summary>
    /// Represents the entrypoint of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the program, called by the OS
        /// </summary>
        /// <param name="args">Command line arguments passed to the program from the OS</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Builds the web host that runs the site
        /// </summary>
        /// <param name="args">Arguments used to configure the builder</param>
        /// <returns>A Web Host configured to run the site</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
