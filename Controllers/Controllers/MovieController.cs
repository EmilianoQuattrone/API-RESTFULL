using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Movie;
using Models.Entities;
using RepositoryPattern.IRepository.Interfaces.IMovie;

namespace Controller.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovies()
        {
            // Lista con datos de Movie de db.
            ICollection<Movie> listMovie = _movieRepository.GetMovies();

            List<MovieDto> listMovieDto = new List<MovieDto>();

            foreach (Movie item in listMovie)
            {
                listMovieDto.Add(_mapper.Map<MovieDto>(item));
            }

            return Ok(listMovieDto);
        }

        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int movieId)
        {
            // Lista con datos de Movie de db.
            Movie movie = _movieRepository.GetMovie(movieId);

            if (movie == null)
                return NotFound();

            MovieDto movieDto = _mapper.Map<MovieDto>(movie);

            return Ok(movieDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createMovieDto == null)
                return BadRequest(ModelState);

            if (_movieRepository.ExistMovie(createMovieDto.NameMovie))
            {
                ModelState.AddModelError("", "La película existe.");
                return StatusCode(404, ModelState);
            }

            Movie createMovie = _mapper.Map<Movie>(createMovieDto);

            if (!_movieRepository.CreateMovie(createMovie))
            {
                ModelState.AddModelError("", $"Hubo un error al guardar el registro {createMovie.NameMovie}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetMovie", new
            {
                movieId = createMovie.Id
            }, createMovie);
        }

        [HttpPatch("{movieId:int}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult UpdateMovie(int movieId, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (movieDto == null || movieId != movieDto.Id)
                return BadRequest(ModelState);

            Movie createMovie = _mapper.Map<Movie>(movieDto);

            if (!_movieRepository.UpdateMovie(createMovie))
            {
                ModelState.AddModelError("", $"Hubo un error al actualizar película {createMovie.NameMovie}");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.ExistMovie(movieId))
                return NotFound();

            Movie deleteMovie = _movieRepository.GetMovie(movieId);

            if (!_movieRepository.DeleteMovie(deleteMovie))
            {
                ModelState.AddModelError("", $"Hubo un error al borrar película {deleteMovie.NameMovie}");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }
    }
}