using Models.Entities;

namespace RepositoryPattern.IRepository.Interfaces.IMovie
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();

        ICollection<Movie> GetMovieByCategory(int categoryId);

        Movie GetMovie(int id);

        bool ExistMovie(int id);

        bool ExistMovie(string nameMovie);

        bool CreateMovie(Movie movie);

        bool UpdateMovie(Movie movie);

        bool DeleteMovie(Movie movie);

        bool Save();
    }
}