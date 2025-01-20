using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Category;
using Models.Entities;
using Service.IServices;

namespace Controller.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            try
            {
                List<CategoryDto> categories = _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            try
            {
                Category category = _categoryService.GetCategory(categoryId);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Este status 401 es para que la Categoria esta bajo autenticacion.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                CategoryDto createdCategory = _categoryService.CreateCategory(createCategoryDto);

                return CreatedAtRoute("GetCategory", new
                {
                    categoryId = createdCategory.Id,
                }, createdCategory);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("La entrada no puede ser nula.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(500, ModelState);
            }
        }

        // HttpPatch (Recomendado usar) nos permite actualizar un campo de un registro.
        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                _categoryService.UpdateCategory(categoryId, categoryDto);

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Los datos de la película no pueden ser nulos.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = ex.Message });
            }
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
            try
            {
                _categoryService.DeleteCategory(categoryId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                { message = ex.Message });
            }
        }
    }
}