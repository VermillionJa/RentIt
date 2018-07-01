using System;
using System.Collections.Generic;
using RentIt.Data.Entities;

namespace RentIt.Services.Repositories
{
    /// <summary>
    /// Represents the interface for a repository for accessing and manipulating Movie resources
    /// </summary>
    public interface IMoviesRepo
    {

        /// <summary>
        /// Adds the given Movie to the Repository
        /// </summary>
        /// <param name="movie">The Movie to add</param>
        void AddMovie(Movie movie);

        /// <summary>
        /// Retrieves all of the Movies
        /// </summary>
        /// <returns>A collection of Movies</returns>
        IEnumerable<Movie> GetAllMovies();

        /// <summary>
        /// Get the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to get</param>
        /// <returns>The Movie with the given Id</returns>
        Movie GetMovieById(int id);

        /// <summary>
        /// Removes the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to remove</param>
        void RemoveMovie(int id);

        /// <summary>
        /// Removes the given Movie
        /// </summary>
        /// <param name="movie">The Movie to remove</param>
        void RemoveMovie(Movie movie);

        /// <summary>
        /// Updates a Movie
        /// </summary>
        /// <param name="movie">The updated Movie data</param>
        void UpdateMovie(Movie movie);

        /// <summary>
        /// Checks to see if a Movie exists with the same Title and Release Date
        /// </summary>
        /// <param name="title">The Title of the Movie</param>
        /// <param name="releaseDate">The Release Date of the Movie</param>
        /// <returns>True if a Movie exists with the same Title and Release Date</returns>
        bool MovieExists(string title, DateTime releaseDate);

        /// <summary>
        /// Gets the Movie Genre with the given name
        /// </summary>
        /// <param name="name">The Name of the Genre</param>
        /// <returns>The Movie Genre with the given Name</returns>
        MovieGenre GetGenreByName(string name);

        /// <summary>
        /// Checks to see if a Genre exists with the same Name
        /// </summary>
        /// <param name="name">The Name of the Genre</param>
        /// <returns>True if a Genre exists with the same Name</returns>
        bool GenreExists(string name);
    }
}