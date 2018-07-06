using Microsoft.EntityFrameworkCore;
using RentIt.Data.Entities;

namespace RentIt.Data
{
    public interface IDbContext
    {
        /// <summary>
        /// The Movies stored in the database
        /// </summary>
        DbSet<Movie> Movies { get; }

        /// <summary>
        /// The Movie Genres stored in the database
        /// </summary>
        DbSet<MovieGenre> MovieGenres { get; }

        int SaveChanges();
    }
}