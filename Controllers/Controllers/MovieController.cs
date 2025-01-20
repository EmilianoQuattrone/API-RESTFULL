using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Movie;
using Models.Entities;
using RepositoryPattern.IRepository.Interfaces.IMovie;
using Service.IServices;

namespace Controller.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService, IMovieRepository movieRepository, IMapper mapper)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovies()
        {
            try
            {
                List<MovieDto> movies = _movieService.GetAllMovies();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { 
                    message = ex.Message });
            }
        }

        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int movieId)
        {
            try
            {
                Movie movie = _movieService.GetMovie(movieId);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            try
            {
                MovieDto createdMovie = _movieService.CreateMovie(createMovieDto);

                return CreatedAtRoute("GetMovie", new 
                { 
                    movieId = createdMovie.Id 
                }, createdMovie);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("La entrada no puede ser nula.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(500, ModelState);
            }
        }

        [HttpPatch("{movieId:int}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult UpdateMovie(int movieId, [FromBody] MovieDto movieDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                _movieService.UpdateMovie(movieId, movieDto);

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Los datos de la película no pueden ser nulos.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = ex.Message });
            }
        }

        [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteMovie(int movieId)
        {
            try
            {
                _movieService.DeleteMovie(movieId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { message = ex.Message });
            }
        }
    }
}