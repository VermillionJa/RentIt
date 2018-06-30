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
using Microsoft.AspNetCore.JsonPatch;

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
        /// Gets all the Movies in the Movie Collection
        /// </summary>
        /// <returns>
        /// 200 - Ok - Returns a collection with all of the Movies
        /// </returns>
        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = _repo.GetAll();
            
            var movieDtos = movies.Select(m => new MovieDto()
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                Rating = m.Rating,
                Genre = m.Genre.Name
            });
            
            return Ok(movieDtos);
        }

        /// <summary>
        /// Gets a Movie from the Movie Collection
        /// </summary>
        /// <param name="id">The Id of the Movie to get</param>
        /// <returns>
        /// 404 - Not Found - If no Movie with the given Id was found
        /// 200 - Ok - Returns a collection with all of the Movies
        /// </returns>
        [HttpGet("{id}", Name = "GetMovie")]
        public IActionResult GetMovie(int id)
        {
            var movie = _repo.GetById(id);
            
            if (movie == null)
            {
                var logMessage = new StringBuilder();

                logMessage.AppendLine("Get Movie - 404 - Not Found");
                logMessage.AppendLine($"The Movie with Id {id} does not exist");

                _logger.LogDebug(logMessage.ToString());

                return NotFound($"Movie with Id {id} does not exist");
            }
            
            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                Genre = movie.Genre.Name
            };

            return Ok(movieDto);
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
        
        /// <summary>
        /// Performs a full update of the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to update</param>
        /// <param name="movieDto">The new data to update the Movie with</param>
        /// <returns>
        /// 400 - Bad Request - If the JSON syntax is invalid
        /// 422 - Unprocessable Entity - If the JSON syntax is valid, but the content is invalid
        /// 404 - Not Found - If no Movie with the given Id was found
        /// 204 - No Content
        /// </returns>
        [BasicAuth("Manager")]
        [HttpPut("{id}")]
        public IActionResult FullUpdateMovie(int id, [FromBody] AddMovieDto movieDto)
        {
            if (movieDto == null)
            {
                _logger.LogDebug("Full Update Movie - 400 - Bad Request");

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

                logMessage.AppendLine("Full Update Movie - 422 - Unprocessable Entity");
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

            var movie = _repo.GetById(id);

            if (movie == null)
            {
                var logMessage = new StringBuilder();

                logMessage.AppendLine("Full Update Movie - 404 - Not Found");
                logMessage.AppendLine($"The Movie with Id {id} does not exist");

                _logger.LogDebug(logMessage.ToString());

                return NotFound($"Movie with Id {id} does not exist");
            }

            movie.Title = movieDto.Title;
            movie.Description = movieDto.Description;
            movie.ReleaseDate = movieDto.ReleaseDate;
            movie.Rating = movieDto.Rating;
            movie.Genre = genre;

            _repo.Update(movie);

            return NoContent();
        }

        /// <summary>
        /// Performs a partial update of the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to update</param>
        /// <param name="patchDoc">A JSON Patch Document with instructions on how to update the Movie</param>
        /// <returns>
        /// 400 - Bad Request - If the JSON syntax is invalid
        /// 422 - Unprocessable Entity - If the JSON syntax is valid, but the content is invalid
        /// 404 - Not Found - If no Movie with the given Id was found
        /// 204 - No Content
        /// </returns>
        [HttpPatch("{id}")]
        [BasicAuth("Manager")]
        public IActionResult PartialUpdateMovie(int id, [FromBody] JsonPatchDocument<AddMovieDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogDebug("Partial Update Movie - 400 - Bad Request");

                return BadRequest(ModelState);
            }
            
            var movie = _repo.GetById(id);

            if (movie == null)
            {
                var logMessage = new StringBuilder();

                logMessage.AppendLine("Partial Update Movie - 404 - Not Found");
                logMessage.AppendLine($"The Movie with Id {id} does not exist");

                _logger.LogDebug(logMessage.ToString());

                return NotFound($"Movie with Id {id} does not exist");
            }
            
            var movieDto = new AddMovieDto
            {
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                Genre = movie.Genre.Name
            };
            
            patchDoc.ApplyTo(movieDto, ModelState);
            
            MovieGenre genre = null;

            if (!string.IsNullOrWhiteSpace(movieDto.Genre))
            {
                genre = _repo.GetGenreByName(movieDto.Genre);

                if (genre == null)
                {
                    ModelState.AddModelError(nameof(movieDto.Genre), $"{movieDto.Genre} is not a valid Genre");
                }
            }
            
            if (!TryValidateModel(movieDto))
            {
                var logMessage = new StringBuilder();

                logMessage.AppendLine("Partial Update Movie - 422 - Unprocessable Entity");
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
            
            movie.Title = movieDto.Title;
            movie.Description = movieDto.Description;
            movie.ReleaseDate = movieDto.ReleaseDate;
            movie.Rating = movieDto.Rating;
            movie.Genre = genre;
            
            _repo.Update(movie);
            
            return NoContent();
        }

        /// <summary>
        /// Removes the Movie with the given Id
        /// </summary>
        /// <param name="id">The Id of the Movie to remove</param>
        /// <returns>
        /// 404 - Not Found - If no Movie with the given Id was found
        /// 204 - No Content
        /// </returns>
        [HttpDelete("{id}")]
        [BasicAuth("Manager")]
        public IActionResult RemoveMovie(int id)
        {
            var movie = _repo.GetById(id);

            if (movie == null)
            {
                var logMessage = new StringBuilder();

                logMessage.AppendLine("Remove Movie - 404 - Not Found");
                logMessage.AppendLine($"The Movie with Id {id} does not exist");

                _logger.LogDebug(logMessage.ToString());

                return NotFound($"Movie with Id {id} does not exist");
            }
            
            _repo.Remove(movie);
            
            return NoContent();
        }
    }
}