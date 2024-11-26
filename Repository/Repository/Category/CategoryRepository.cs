using Data.Context;
using RepositoryPattern.IRepository.Interfaces.ICategory;

namespace RepositoryPattern.Repository.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        // Esta propiedad contiene el acceso a cada de las entidades del proyecto.
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool CreateCategory(Models.Entities.Category category)
        {
            DateTime TodayDate = DateTime.Now;
            category.CreationDate = TodayDate;
            _applicationDbContext.Categories.Add(category);

            return Save();
        }

        public bool DeleteCategory(Models.Entities.Category category)
        {
            _applicationDbContext.Categories.Remove(category);
            return Save();
        }

        public bool ExistCategory(int id)
        {
            return _applicationDbContext.Categories.Any(c => c.Id == id);
        }

        public bool ExistCategory(string nameCategory)
        {
            bool result;
            result = _applicationDbContext.Categories.Any(c =>
                                          c.NameCategory.ToLower().Trim() == nameCategory);
            return result;
        }

        public ICollection<Models.Entities.Category> GetCategories()
        {
            return _applicationDbContext.Categories.OrderBy(c => c.NameCategory).ToList();
        }

        public Models.Entities.Category GetCategory(int id)
        {
            return _applicationDbContext.Categories.FirstOrDefault(c =>
            c.Id == id);
        }

        public bool Save()
        {
            return _applicationDbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Models.Entities.Category category)
        {
            DateTime TodayDate = DateTime.Now;
            category.CreationDate = TodayDate;
            Models.Entities.Category categoryExists = _applicationDbContext.Categories.Find(category.Id);

            if (categoryExists != null)
            {
                _applicationDbContext.Entry(categoryExists).CurrentValues.SetValues(category);
            }
            else
            {
                _applicationDbContext.Categories.Update(category);
            }

            return Save();
        }
    }
}