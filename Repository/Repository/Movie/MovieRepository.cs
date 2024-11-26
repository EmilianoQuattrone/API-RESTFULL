using Data.Context;
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.IRepository.Interfaces.IMovie;

namespace RepositoryPattern.Repository.Movie
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MovieRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool CreateMovie(Models.Entities.Movie movie)
        {
            DateTime TodayDate = DateTime.Now;
            movie.CreationDate = TodayDate;
            _applicationDbContext.Movies.Add(movie);

            return Save();
        }

        public bool DeleteMovie(Models.Entities.Movie movie)
        {
            _applicationDbContext.Movies.Remove(movie);
            return Save();
        }

        public bool ExistMovie(int id)
        {
            return _applicationDbContext.Movies.Any(m => m.Id == id);
        }

        public bool ExistMovie(string nameMovie)
        {
            bool result;
            result = _applicationDbContext.Movies.Any(m =>
                                          m.NameMovie.ToLower().Trim() == nameMovie);
            return result;
        }

        public Models.Entities.Movie GetMovie(int id)
        {
            return _applicationDbContext.Movies.FirstOrDefault(m =>
            m.Id == id);
        }

        public ICollection<Models.Entities.Movie> GetMovieByCategory(int categoryId)
        {
            return _applicationDbContext.Movies.Include(c => c.Category)
                                               .Where(ca => ca.categoryId == categoryId)
                                               .ToList();
        }

        public ICollection<Models.Entities.Movie> GetMovies()
        {
            return _applicationDbContext.Movies.OrderBy(m => m.NameMovie).ToList();
        }

        public bool Save()
        {
            return _applicationDbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateMovie(Models.Entities.Movie movie)
        {
            DateTime TodayDate = DateTime.Now;
            movie.CreationDate = TodayDate;
            Models.Entities.Movie movieExists = _applicationDbContext.Movies.Find(movie.Id);

            if (movieExists != null)
            {
                _applicationDbContext.Entry(movieExists).CurrentValues.SetValues(movie);
            }
            else
            {
                _applicationDbContext.Movies.Update(movie);
            }

            return Save();
        }
    }
}