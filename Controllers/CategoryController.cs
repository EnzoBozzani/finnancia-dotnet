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
                    existingCategory = await _categoryRepository.GetCategory(bodyDTO, userId);
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
    }
}