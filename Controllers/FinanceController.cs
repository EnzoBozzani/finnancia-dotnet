using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;

namespace FinnanciaCSharp.Controllers
{
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

                var financesWithCategories = await _financeRepository.GetFinancesWithCategories(sheetId, userId);

                return Ok(financesWithCategories);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}