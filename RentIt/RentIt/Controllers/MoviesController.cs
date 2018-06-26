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
using System.Text;
using RentIt.RequestFilters.Auth;

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
        [BasicAuth("Manager")]
        public IActionResult AddMovie([FromBody] AddMovieDto movieDto)
        {
            if (movieDto == null)
            {
                _logger.LogDebug("Add Movie - 400 - Bad Request");

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
                var logMessage = new StringBuilder();
                
                logMessage.AppendLine("Add Movie - 422 - Unprocessable Entity");
                logMessage.AppendLine("Errors:");

                foreach (var entry in ModelState)
                {
                    logMessage.AppendLine(entry.Key);

                    foreach (var error in entry.Value.Errors)
                    {
                        logMessage.AppendLine($"\t{error.ErrorMessage}");
                    }
                }
                
                _logger.LogDebug(logMessage.ToString());


                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (_repo.Exists(movieDto.Title, movieDto.ReleaseDate))
            {
                var logMessage = new StringBuilder();

                logMessage.AppendLine("Add Movie - 409 - Conflict");
                logMessage.AppendLine($"The Movie '{movieDto.Title}' released on {movieDto.ReleaseDate.ToShortDateString()} already exists");

                _logger.LogDebug(logMessage.ToString());

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

            var successLogMessage = new StringBuilder();

            successLogMessage.AppendLine("Add Movie - 201 - Created");
            successLogMessage.AppendLine($"The Movie '{movieDto.Title}' released on {movieDto.ReleaseDate.ToShortDateString()} was successfully added");

            _logger.LogInformation(successLogMessage.ToString());

            return CreatedAtRoute("GetMovie", new { Id = returnDto.Id }, returnDto);
        }
    }
}