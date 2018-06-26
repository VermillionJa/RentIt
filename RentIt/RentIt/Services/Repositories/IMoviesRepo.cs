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
        void Add(Movie movie);

        /// <summary>
        /// Retrieves all of the Movies
        /// </summary>
        /// <returns>A collection of Movies</returns>
        IEnumerable<Movie> GetAll();

        /// <summary>
        /// Get the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to get</param>
        /// <returns>The Movie with the given Id</returns>
        Movie GetById(int id);

        /// <summary>
        /// Gets the Movie Genre with the given name
        /// </summary>
        /// <param name="genre">The name of the Genre</param>
        /// <returns>The Movie Genre with the given name</returns>
        MovieGenre GetGenreByName(string genre);

        /// <summary>
        /// Removes the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to remove</param>
        void Remove(int id);

        /// <summary>
        /// Removes the given Movie
        /// </summary>
        /// <param name="movie">The Movie to remove</param>
        void Remove(Movie movie);

        /// <summary>
        /// Updates a Movie
        /// </summary>
        /// <param name="movie">The updated Movie data</param>
        void Update(Movie movie);

        /// <summary>
        /// Checks to see if a Movie exists with the same Title and Release Date
        /// </summary>
        /// <param name="title">The Title of the Movie</param>
        /// <param name="releaseDate">The Release Date of the Movie</param>
        /// <returns>True if a Movie exists with the same Title and Release Date</returns>
        bool Exists(string title, DateTime releaseDate);
    }
}