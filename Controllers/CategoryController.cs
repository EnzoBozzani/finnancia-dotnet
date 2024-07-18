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
        private readonly IUserSubscriptionRepository _userSubRepository;
        public CategoryController(UserManager<User> userManager, ICategoryRepository categoryRepository, IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _userSubRepository = userSubscriptionRepository;
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

                var userSubscription = await _userSubRepository.GetUserSubscriptionAsync(userId);

                var categoriesCount = await _categoryRepository.CountAsync(userId);

                if (categoriesCount >= Constants.Constants.MAX_CATEGORIES_FOR_FREE && (userSubscription == null || !userSubscription.IsActive))
                {
                    return Ok(new { message = "Você atingiu o limite de 5 categorias!", maxFreeCategoriesReached = true });
                }

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

        [HttpGet("colors")]
        public async Task<IActionResult> GetColors()
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

                var colors = categories.Select(category => category.Color);

                return Ok(colors);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var category = await _categoryRepository.DeleteAsync(id);

                if (category == null)
                {
                    return NotFound(new { error = "Categoria não encontrada" });
                }

                return Ok(new { success = "Categoria deletada com sucesso!" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}