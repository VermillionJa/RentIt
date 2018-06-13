using Microsoft.EntityFrameworkCore;
using RentIt.Data.Entities;
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
        /// The Customers stored in the database
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// The Inventory Items stored in the database
        /// </summary>
        public DbSet<InventoryItem> InventoryItems { get; set; }

        /// <summary>
        /// The Movies stored in the database
        /// </summary>
        public DbSet<Movie> Movies { get; set; }

        /// <summary>
        /// The Movie Genres stored in the database
        /// </summary>
        public DbSet<MovieGenre> MovieGenres { get; set; }

        /// <summary>
        /// The Rental Transactions stored in the database
        /// </summary>
        public DbSet<RentalTransaction> RentalTransactions { get; set; }

        /// <summary>
        /// Initializes a new instance of the AppDbContext class configured using the given options
        /// </summary>
        /// <param name="options">The options to configure the context with</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }
    }
}
