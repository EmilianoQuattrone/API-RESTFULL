using Models.DTOs.Category;
using Models.Entities;

namespace Service.IServices
{
    public interface ICategoryService
    {
        List<CategoryDto> GetAllCategories();

        Category GetCategory(int id);

        CategoryDto CreateCategory(CreateCategoryDto createCategoryDto);

        bool UpdateCategory(int categoryId, CategoryDto categoryDto);

        bool DeleteCategory(int categoryId);

        bool Save();
    }
}