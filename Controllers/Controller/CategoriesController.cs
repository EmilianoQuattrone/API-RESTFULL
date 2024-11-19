using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Entities;
using Repository.IRepository;

namespace Controllers.Controller
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository,
                                  IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            // Lista con datos de Category de db.
            ICollection<Category> listCategory = _categoryRepository.GetCategories();

            List<CategoryDto> listCategoryDto = new List<CategoryDto>();

            foreach (Category item in listCategory)
            {
                listCategoryDto.Add(_mapper.Map<CategoryDto>(item));
            }

            return Ok(listCategoryDto);
        }
    }
}