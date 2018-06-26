using Microsoft.EntityFrameworkCore;
using RentIt.Data;
using RentIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Services.Repositories
{
    /// <summary>
    /// Represents a repository for accessing and manipulating Movie resources
    /// </summary>
    public class MoviesRepo : IMoviesRepo
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the MoviesRepo class
        /// </summary>
        /// <param name="context">The AppDbContext injected via DI</param>
        public MoviesRepo(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all of the Movies
        /// </summary>
        /// <returns>A collection of Movies</returns>
        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.Include(m => m.Genre);
        }

        /// <summary>
        /// Get the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to get</param>
        /// <returns>The Movie with the given Id</returns>
        public Movie GetById(int id)
        {
            return _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Adds the given Movie to the Repository
        /// </summary>
        /// <param name="movie">The Movie to add</param>
        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates a Movie
        /// </summary>
        /// <param name="movie">The updated Movie data</param>
        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to remove</param>
        public void Remove(int id)
        {
            Remove(GetById(id));
        }

        /// <summary>
        /// Removes the given Movie
        /// </summary>
        /// <param name="movie">The Movie to remove</param>
        public void Remove(Movie movie)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets the Movie Genre with the given name
        /// </summary>
        /// <param name="genre">The name of the Genre</param>
        /// <returns>The Movie Genre with the given name</returns>
        public MovieGenre GetGenreByName(string genre)
        {
            return _context.MovieGenres.FirstOrDefault(g => g.Name.ToUpper() == genre.ToUpper());
        }

        /// <summary>
        /// Checks to see if a Movie exists with the same Title and Release Date
        /// </summary>
        /// <param name="title">The Title of the Movie</param>
        /// <param name="releaseDate">The Release Date of the Movie</param>
        /// <returns>True if a Movie exists with the same Title and Release Date</returns>
        public bool Exists(string title, DateTime releaseDate)
        {
            return _context.Movies.Any(m => m.Title.ToUpper() == title.ToUpper() && m.ReleaseDate.Date == releaseDate.Date);
        }
    }
}
