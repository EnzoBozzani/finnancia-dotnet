using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.DTOs.Finance;
using Microsoft.AspNetCore.Authorization;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/finances")]
    public class FinanceController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IFinanceRepository _financeRepository;
        public FinanceController(UserManager<User> userManager, IFinanceRepository financeRepository)
        {
            _userManager = userManager;
            _financeRepository = financeRepository;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditFinance([FromRoute] Guid id, [FromBody] EditFinanceDTO bodyDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                if (bodyDTO.Amount == null && bodyDTO.CategoryId == null && bodyDTO.Date == null && bodyDTO.Title == null && bodyDTO.FinanceType == null)
                {
                    return BadRequest(new { error = "Pelo menos 1 campo é obrigatório" });
                }

                var finance = await _financeRepository.UpdateAsync(user, id, bodyDTO);

                if (finance == null)
                {
                    return BadRequest(new { error = "Campo(s) inválido(s)" });
                }

                return Ok(new { success = "Editado com sucesso!" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinance([FromRoute] Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var finance = await _financeRepository.DeleteAsync(user, id);

                if (finance == null)
                {
                    return NotFound(new { error = "Finança ou planilha não encontrada(s)" });
                }

                return Ok(new { success = "Deletado com sucesso!" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpGet("categories/{sheetId}")]
        public async Task<IActionResult> GetFinancesWithCategories([FromRoute] Guid sheetId)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var financesWithCategories = await _financeRepository.GetFinancesWithCategoriesAsync(sheetId, userId);

                return Ok(financesWithCategories);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}