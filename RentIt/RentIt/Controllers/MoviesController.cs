using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentIt.Services.Repositories;
using Microsoft.Extensions.Logging;
using RentIt.Models;
using RentIt.Models.ObjectResults;
using RentIt.Data.Entities;
using RentIt.Models.Movies;

namespace RentIt.Controllers
{
    /// <summary>
    /// An API Controller for accessing and manipulating Movie resources
    /// </summary>
    [Route("api/Movies")]
    [Produces("application/json")]
    public class MoviesController : Controller
    {
        private readonly IMoviesRepo _repo;
        private readonly ILogger<MoviesController> _logger;

        /// <summary>
        /// Initializes a new instance of the MoviesController
        /// </summary>
        /// <param name="repo">The Movies Repository injected via DI</param>
        /// <param name="logger">The Logger Service injected via DI</param>
        public MoviesController(IMoviesRepo repo, ILogger<MoviesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Adds a Movie to the Movie Collection
        /// </summary>
        /// <param name="movieDto">The data to use for adding the new Movie</param>
        /// <returns>
        /// 400 - Bad Request - If the JSON syntax is invalid
        /// 422 - Unprocessable Entity - If the JSON syntax is valid, but the content is invalid
        /// 201 - Created - If the Movie was successfully added, also returns the URL to GET the new Movie at and the Movie object
        /// </returns>
        [HttpPost]
        public IActionResult AddMovie([FromBody] AddMovieDto movieDto)
        {
            if (movieDto == null)
            {
                return BadRequest(ModelState);
            }

            MovieGenre genre = null;

            if (!string.IsNullOrWhiteSpace(movieDto.Genre))
            {
                genre = _repo.GetGenreByName(movieDto.Genre);

                if (genre == null)
                {
                    ModelState.AddModelError(nameof(movieDto.Genre), $"{movieDto.Genre} is not a valid Genre");
                }
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (_repo.Exists(movieDto.Title, movieDto.ReleaseDate))
            {
                return new ConflictObjectResult($"The Movie '{movieDto.Title}' released on {movieDto.ReleaseDate.ToShortDateString()} already exists");
            }

            var movie = new Movie
            {
                Title = movieDto.Title,
                Description = movieDto.Description,
                ReleaseDate = movieDto.ReleaseDate,
                Rating = movieDto.Rating,
                Genre = genre
            };

            _repo.Add(movie);

            var returnDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                Genre = movie.Genre.Name
            };

            return CreatedAtRoute("GetMovie", new { Id = returnDto.Id }, returnDto);
        }
    }
}