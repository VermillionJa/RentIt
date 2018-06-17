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
    }
}