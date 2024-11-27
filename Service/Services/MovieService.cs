using AutoMapper;
using Data.Context;
using Models.DTOs.Movie;
using Models.Entities;
using RepositoryPattern.IRepository.Interfaces.IMovie;
using Service.IServices;

namespace Service.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieService(ApplicationDbContext applicationDbContex, 
                            IMovieRepository movieRepository, IMapper mapper)
        {
            _applicationDbContext = applicationDbContex;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public MovieDto CreateMovie(CreateMovieDto createMovieDto)
        {
            if (createMovieDto == null)
                throw new ArgumentNullException(nameof(createMovieDto));

            if (_movieRepository.ExistMovie(createMovieDto.NameMovie))
                throw new InvalidOperationException("La película ya existe.");

            Movie createMovie = _mapper.Map<Movie>(createMovieDto);

            if (!_movieRepository.CreateMovie(createMovie))
                throw new Exception($"Hubo un error al guardar el registro {createMovie.NameMovie}");

            return _mapper.Map<MovieDto>(createMovie);
        }

        public List<MovieDto> GetAllMovies()
        {
            ICollection<Movie> listMovie = _movieRepository.GetMovies();

            List<MovieDto> listMovieDto = listMovie
                .Select(movie => _mapper.Map<MovieDto>(movie))
                .ToList();

            return listMovieDto;
        }

        public Movie GetMovie(int id)
        {
            Movie movie = _movieRepository.GetMovie(id);

            if (!_movieRepository.ExistMovie(movie.Id))
                throw new InvalidOperationException("No se encontro la película.");

            MovieDto movieDto = _mapper.Map<MovieDto>(movie);

            return movie;
        }

        public bool UpdateMovie(int movieId, MovieDto movieDto)
        {
            if (movieDto == null)
                throw new ArgumentNullException(nameof(movieDto), 
                    "Los datos de la película no pueden ser nulos.");

            if (movieId != movieDto.Id)
                throw new ArgumentException("El ID de la película no coincide.");

            Movie movieUpdate = _mapper.Map<Movie>(movieDto);

            if (!_movieRepository.UpdateMovie(movieUpdate))
                throw new Exception($"Hubo un error al actualizar la película: " +
                                    $"{movieUpdate.NameMovie}");

            return Save();
        }

        public bool DeleteMovie(int movieId)
        {
            if (!_movieRepository.ExistMovie(movieId))
                throw new KeyNotFoundException($"La película con ID {movieId} no existe.");

            Movie movieToDelete = _movieRepository.GetMovie(movieId);

            if (movieToDelete == null)
                throw new KeyNotFoundException($"La película con ID {movieId} " +
                                               $"no se pudo encontrar para eliminar.");

            // Intentar eliminar la película
            if (!_movieRepository.DeleteMovie(movieToDelete))
                throw new Exception($"Hubo un error al borrar la película: " +
                                    $"{movieToDelete.NameMovie}");

            return Save();
        }

        public bool Save()
        {
            return _applicationDbContext.SaveChanges() >= 0 ? true : false;
        }
    }
}