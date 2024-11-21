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

        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            // Lista con datos de Category de db.
            Category category = _categoryRepository.GetCategory(categoryId);

            if (category == null)
                return NotFound();

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
            
            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // Este status 401 es para que la Categoria esta bajo autenticacion.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createCategoryDto == null)
                return BadRequest(ModelState);

            if (_categoryRepository.ExistCategory(createCategoryDto.NameCategory))
            {
                ModelState.AddModelError("", "La categoría existe.");
                return StatusCode(404, ModelState);
            }

            Category createCategory = _mapper.Map<Category>(createCategoryDto);

            if (!_categoryRepository.CreateCategory(createCategory))
            {
                ModelState.AddModelError("", $"Hubo un error al guardar el registro {createCategory.NameCategory}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCategory", new 
            {
                categoryId = createCategory.Id
            },  createCategory);
        }

        // HttpPatch (Recomendado usar) nos permite actualizar un campo de un registro.
        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (categoryDto == null || categoryId != categoryDto.Id)
                return BadRequest(ModelState);

            Category createCategory = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.UpdateCategory(createCategory))
            {
                ModelState.AddModelError("", $"Hubo un error al actualizar categoría {createCategory.NameCategory}");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }

        /*
          A modo de práctica hice un borrado directo, lo ideal sería hacer un borrado lógico,
          que se resolvería agregando a la entidad Category un campo binario por ej: bitBorrado 
          con un valor de 0 en caso de encontrarse disponible, y en caso de no estarlo el valor sea 1.
        */

        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.ExistCategory(categoryId))
                return NotFound();

            Category deleteCategory = _categoryRepository.GetCategory(categoryId);

            if (!_categoryRepository.DeleteCategory(deleteCategory))
            {
                ModelState.AddModelError("", $"Hubo un error al borrar categoría {deleteCategory.NameCategory}");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }
    }
}