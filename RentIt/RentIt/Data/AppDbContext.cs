using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Data
{
    /// <summary>
    /// Represents the Entity Framework interface for accessing entities stored in the database
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the AppDbContext class configured using the given options
        /// </summary>
        /// <param name="options">The options to configure the context with</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
