using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.DTOs.Category;
using FinnanciaCSharp.Interfaces;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(UserManager<User> userManager, ICategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var categories = await _categoryRepository.GetCategoriesAsync(userId);

                return Ok(categories);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] NewCategoryDTO bodyDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                //TODO: User Subscription and count categories

                Category? existingCategory = null;

                if (bodyDTO.Color != "transparent")
                {
                    existingCategory = await _categoryRepository.GetCategoryAsync(bodyDTO, userId);
                }

                if (existingCategory != null)
                {
                    return BadRequest(new { error = $"Categoria {existingCategory.Name} já existe!" });
                }

                await _categoryRepository.CreateAsync(bodyDTO, userId);

                return Ok(new { success = "Categoria criada com sucesso!" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryDTO bodyDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                if (bodyDTO.Color == null && bodyDTO.Name == null)
                {
                    return BadRequest(new { error = "Pelo menos 1 campo é obrigatório" });
                }

                var category = await _categoryRepository.UpdateAsync(bodyDTO, id);

                if (category == null)
                {
                    return NotFound(new { error = "Categoria não encontrada" });
                }

                return Ok(new { success = "Categoria editada com sucesso!" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}