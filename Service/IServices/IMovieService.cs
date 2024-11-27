using Models.DTOs.Movie;
using Models.Entities;

namespace Service.IServices
{
    public interface IMovieService
    {
        List<MovieDto> GetAllMovies();

        Movie GetMovie(int id);

        MovieDto CreateMovie(CreateMovieDto createMovieDto);

        bool UpdateMovie(int movieId, MovieDto movieDto);

        bool DeleteMovie(int movieId);

        bool Save();
    }
}