using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RentIt.Data;
using Microsoft.EntityFrameworkCore;

namespace RentIt
{
    /// <summary>
    /// Represents the configuration that the Web Host uses on startup
    /// </summary>
    public class Startup
    {
        private IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the Startup class
        /// </summary>
        /// <param name="configuration">The site configuration</param>
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
                
        /// <summary>
        /// Adds services to the Dependency Injection Services container
        /// </summary>
        /// <param name="services">The Dependency Injection Services container</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(_config.GetConnectionString("Default")));

            services.AddMvc();
        }

        /// <summary>
        /// Adds middleware to the Http Request pipeline
        /// </summary>
        /// <param name="app">The Application Builder that adds the middleware</param>
        /// <param name="env">The Hosting Environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
