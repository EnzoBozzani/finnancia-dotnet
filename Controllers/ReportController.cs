using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.DTOs.Finance;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISheetRepository _sheetRepository;
        public ReportController(UserManager<User> userManager, ISheetRepository sheetRepository)
        {
            _userManager = userManager;
            _sheetRepository = sheetRepository;
        }

        [HttpGet("{sheetId}")]
        public async Task<IActionResult> GetDataForReport([FromRoute] Guid sheetId)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                //TODO: User Sub

                // if (user.HasUsedFreeReport && !hasActiveSubscription)
                // {
                //      return Ok(new { message = "Você já usou seu relatório gratuito!", freeReportUsed = "true" });
                // }

                // if (!hasActiveSubscription)
                // {
                //      update user.HasUsedFreeReport para true
                // }

                var sheet = await _sheetRepository.GetSheetWithFinanceWithCategoryAsync(userId, sheetId);

                if (sheet == null)
                {
                    return NotFound(new { error = "Planilha não encontrada!" });
                }

                if (sheet.Finances.Count() == 0)
                {
                    return Ok(new { error = "Não é possível exportar uma planilha vazia!" });
                }

                var sheets = await _sheetRepository.GetSheetWithFinancesAsync(userId);

                var numberOfSheets = sheets.Count == 0 ? 1 : sheets.Count;

                decimal totalExpense = 0;
                decimal totalProfit = 0;
                decimal totalAmount = 0;

                foreach (SheetWithFinanceDTO currentSheet in sheets)
                {
                    totalAmount += currentSheet.TotalAmount;

                    foreach (FinanceDTO finance in currentSheet.Finances)
                    {
                        if (finance.Type == "EXPENSE")
                        {
                            totalExpense += finance.Amount;
                        }
                        else
                        {
                            totalProfit += finance.Amount;
                        }
                    }
                }

                var avgAmount = totalAmount / numberOfSheets;
                var avgProfit = totalProfit / numberOfSheets;
                var avgExpense = totalExpense / numberOfSheets;

                return Ok(new { sheet, avgAmount, avgProfit, avgExpense });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}