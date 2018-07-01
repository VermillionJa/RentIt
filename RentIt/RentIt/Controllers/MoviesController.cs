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
using RentIt.Extensions;
using AutoMapper;

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
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the MoviesController
        /// </summary>
        /// <param name="repo">The Movies Repository injected via DI</param>
        /// <param name="logger">The Logger Service injected via DI</param>
        /// <param name="mapper">The AutoMapper Mapper injected via DI</param>
        public MoviesController(IMoviesRepo repo, ILogger<MoviesController> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
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
            var movies = _repo.GetAllMovies();
            
            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);
            
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
            var movie = _repo.GetMovieById(id);
            
            if (movie == null)
            {
                _logger.LogStrideDebug("Failed to Get Movie", d => d.Add("Status Code", StatusCodes.Status404NotFound).Add("Movie Id", id));
                
                return NotFound($"Movie with Id {id} does not exist");
            }

            var movieDto = _mapper.Map<MovieDto>(movie);

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
                _logger.LogStrideDebug("Failed to Add Movie", d => d.Add("Status Code", StatusCodes.Status400BadRequest).AddModelState(ModelState));

                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(movieDto.Genre))
            {
                if (!_repo.GenreExists(movieDto.Genre))
                {
                    ModelState.AddModelError(nameof(movieDto.Genre), $"{movieDto.Genre} is not a valid Genre");
                }
            }

            if (!ModelState.IsValid)
            {
                _logger.LogStrideDebug("Failed to Add Movie", d => d.Add("Status Code", StatusCodes.Status422UnprocessableEntity).AddModelState(ModelState));

                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (_repo.MovieExists(movieDto.Title, movieDto.ReleaseDate))
            {
                _logger.LogStrideDebug("Failed to Add Movie", d => d.Add("Status Code", StatusCodes.Status409Conflict)
                                                                    .Add("Movie Title", movieDto.Title)
                                                                    .Add("Movie Release Date", movieDto.ReleaseDate.ToShortDateString()));
                
                return new ConflictObjectResult($"The Movie '{movieDto.Title}' released on {movieDto.ReleaseDate.ToShortDateString()} already exists");
            }

            var movie = _mapper.Map<Movie>(movieDto);

            _repo.AddMovie(movie);

            var returnDto = _mapper.Map<MovieDto>(movie);
            
            _logger.LogStrideInformation("Successfully Added Movie", d => d.Add("Status Code", StatusCodes.Status201Created)
                                                                           .Add("Movie Id", returnDto.Id)
                                                                           .Add("Movie Title", returnDto.Title)
                                                                           .Add("Movie Release Date", returnDto.ReleaseDate.ToShortDateString()));
            
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
                _logger.LogStrideDebug("Failed to Fully Update Movie", d => d.Add("Status Code", StatusCodes.Status400BadRequest).AddModelState(ModelState));

                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(movieDto.Genre))
            {
                if (!_repo.GenreExists(movieDto.Genre))
                {
                    ModelState.AddModelError(nameof(movieDto.Genre), $"{movieDto.Genre} is not a valid Genre");
                }
            }

            if (!ModelState.IsValid)
            {
                _logger.LogStrideDebug("Failed to Fully Update Movie", d => d.Add("Status Code", StatusCodes.Status422UnprocessableEntity).AddModelState(ModelState));
                
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var movie = _repo.GetMovieById(id);

            if (movie == null)
            {
                _logger.LogStrideDebug("Failed to Fully Update Movie", d => d.Add("Status Code", StatusCodes.Status404NotFound).Add("Movie Id", id));
                
                return NotFound($"Movie with Id {id} does not exist");
            }

            _mapper.Map(movieDto, movie);

            _repo.UpdateMovie(movie);
            
            _logger.LogStrideInformation("Successfully Fully Updated Movie", d => d.Add("Status Code", StatusCodes.Status204NoContent)
                                                                                   .Add("Movie Id", movie.Id));

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
                _logger.LogStrideDebug("Failed to Partially Update Movie", d => d.Add("Status Code", StatusCodes.Status400BadRequest).AddModelState(ModelState));

                return BadRequest(ModelState);
            }
            
            var movie = _repo.GetMovieById(id);

            if (movie == null)
            {
                _logger.LogStrideDebug("Failed to Partially Update Movie", d => d.Add("Status Code", StatusCodes.Status404NotFound).Add("Movie Id", id));

                return NotFound($"Movie with Id {id} does not exist");
            }

            var movieDto = _mapper.Map<AddMovieDto>(movie);
            
            patchDoc.ApplyTo(movieDto, ModelState);

            if (!string.IsNullOrWhiteSpace(movieDto.Genre))
            {
                if (!_repo.GenreExists(movieDto.Genre))
                {
                    ModelState.AddModelError(nameof(movieDto.Genre), $"{movieDto.Genre} is not a valid Genre");
                }
            }

            if (!TryValidateModel(movieDto))
            {
                _logger.LogStrideDebug("Failed to Partially Update Movie", d => d.Add("Status Code", StatusCodes.Status422UnprocessableEntity).AddModelState(ModelState));
                
                return new UnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(movieDto, movie);

            _repo.UpdateMovie(movie);

            _logger.LogStrideInformation("Successfully Partially Updated Movie", d => d.Add("Status Code", StatusCodes.Status204NoContent)
                                                                                       .Add("Movie Id", movie.Id));

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
            var movie = _repo.GetMovieById(id);

            if (movie == null)
            {
                _logger.LogStrideDebug("Failed to Remove Movie", d => d.Add("Status Code", StatusCodes.Status404NotFound).Add("Movie Id", id));

                return NotFound($"Movie with Id {id} does not exist");
            }
            
            _repo.RemoveMovie(movie);

            _logger.LogStrideInformation("Successfully Removed Movie", d => d.Add("Status Code", StatusCodes.Status204NoContent)
                                                                             .Add("Movie Id", movie.Id));

            return NoContent();
        }
    }
}