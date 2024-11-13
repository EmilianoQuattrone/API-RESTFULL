using Data.Context;
using Models.Entities;
using Repository.IRepository;

namespace RepositoryPattern.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        // Esta propiedad contiene el acceso a cada de las entidades del proyecto.
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool CreateCategory(Category category)
        {
            DateTime TodayDate = DateTime.Now;
            category.CreationDate = TodayDate;
            _applicationDbContext.Category.Add(category);

            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _applicationDbContext.Category.Remove(category);
            return Save();
        }

        public bool ExistCategory(int id)
        {
            return _applicationDbContext.Category.Any(c => c.Id == id);
        }

        public bool ExistCategory(string nameCategory)
        {
            bool result;
            result = _applicationDbContext.Category.Any(c =>
                                          c.NameCategory.ToLower().Trim() == nameCategory);
            return result;
        }

        public ICollection<Category> GetCategories()
        {
            return _applicationDbContext.Category.OrderBy(c => c.NameCategory).ToList();
        }

        public Category GetCategory(int id)
        {
            return _applicationDbContext.Category.FirstOrDefault(c =>
            c.Id == id);
        }

        public bool Save()
        {
            return _applicationDbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            DateTime TodayDate = DateTime.Now;
            category.CreationDate = TodayDate;
            _applicationDbContext.Category.Update(category);

            return Save();
        }
    }
}