using AutoMapper;
using Data.Context;
using Models.DTOs.Category;
using Models.Entities;
using RepositoryPattern.IRepository.Interfaces.ICategory;
using Service.IServices;

namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext applicationDbContex,
                               ICategoryRepository categoryRepository, IMapper mapper)
        {
            _applicationDbContext = applicationDbContex;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public CategoryDto CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
                throw new ArgumentNullException(nameof(createCategoryDto));

            if (_categoryRepository.ExistCategory(createCategoryDto.NameCategory))
                throw new InvalidOperationException("La categoría ya existe.");

            Category createCategory = _mapper.Map<Category>(createCategoryDto);

            if (!_categoryRepository.CreateCategory(createCategory))
                throw new Exception($"Hubo un error al guardar el registro {createCategory.NameCategory}");

            return _mapper.Map<CategoryDto>(createCategory);
        }

        public List<CategoryDto> GetAllCategories()
        {
            ICollection<Category> listCategory = _categoryRepository.GetCategories();

            List<CategoryDto> listCategoryDto = listCategory
                .Select(c => _mapper.Map<CategoryDto>(c))
                .ToList();

            return listCategoryDto;
        }

        public Category GetCategory(int id)
        {
            Category category = _categoryRepository.GetCategory(id);

            if(!_categoryRepository.ExistCategory(category.Id))
                throw new InvalidOperationException("No se encontro la categoría.");

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);

            return category;
        }

        public bool UpdateCategory(int categoryId, CategoryDto categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto),
                    "Los datos de la categoría no pueden ser nulos.");

            if (categoryId != categoryDto.Id)
                throw new ArgumentException("El ID de la categoría no coincide.");

            Category updateCategory = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.UpdateCategory(updateCategory))
                throw new Exception($"Hubo un error al actualizar la categoría: " +
                                    $"{updateCategory.NameCategory}");

            return Save();
        }

        public bool DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.ExistCategory(categoryId))
                throw new KeyNotFoundException($"La categoría con ID {categoryId} no existe.");

            Category categoryDelete = _categoryRepository.GetCategory(categoryId);

            if (categoryDelete == null)
                throw new KeyNotFoundException($"La categoría con ID {categoryId} " +
                                               $"no se pudo encontrar para eliminar.");

            // Intentar eliminar la categoría.
            if (!_categoryRepository.DeleteCategory(categoryDelete))
                throw new Exception($"Hubo un error al borrar la categoría: " +
                                    $"{categoryDelete.NameCategory}");

            return Save();
        }

        public bool Save()
        {
            return _applicationDbContext.SaveChanges() >= 0 ? true : false;
        }
    }
}