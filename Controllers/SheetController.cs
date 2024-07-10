using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.DTOs.Finance;
using FinnanciaCSharp.Mappers;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FinnanciaCSharp.Models;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.Lib;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/sheet")]
    public class SheetController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISheetRepository _sheetRepository;
        private readonly IFinanceRepository _financeRepository;
        public SheetController(ISheetRepository sheetRepository, UserManager<User> userManager, IFinanceRepository financeRepository)
        {
            _sheetRepository = sheetRepository;
            _userManager = userManager;
            _financeRepository = financeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSheets()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var sheets = await _sheetRepository.GetSheetsByUserIdAsync(userId);

                return Ok(new { sheets, isInitialAmountSet = user.IsInitialAmountSet });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSheet([FromBody] CreateSheetDTO createSheetDTO)
        {
            var currentDate = DateTime.Now;

            var year = createSheetDTO.Year;
            var month = createSheetDTO.Month;

            if (year < currentDate.Year || year > currentDate.Year + 1)
            {
                return BadRequest(new { error = "O ano deve ser entre o ano atual ou o próximo" });
            }

            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                if (!user.IsInitialAmountSet)
                {
                    return BadRequest(new { error = "Saldo inicial ainda não foi definido" });
                }

                var sheetExists = await _sheetRepository.SheetExistsByMonthAndYearAsync(month, year, userId);

                if (sheetExists)
                {
                    return BadRequest(new { error = "Planilha já existente" });
                }

                var newSheet = createSheetDTO.ToSheetFromCreateSheetDTO(userId);

                await _sheetRepository.CreateAsync(newSheet);

                return CreatedAtAction(nameof(GetSheetById), new { id = newSheet.Id }, newSheet.ToSheetDTOFromSheet());
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSheetById([FromRoute] Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var sheet = await _sheetRepository.GetSheetByIdAsync(id, userId);

                if (sheet == null)
                {
                    return NotFound(new { error = "Planilha não encontrada!" });
                }

                return Ok(sheet.ToSheetDTOFromSheet());
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSheet([FromRoute] Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var sheet = await _sheetRepository.DeleteSheetAsync(id);

                if (sheet == null)
                {
                    return NotFound(new { error = "Planilha não encontrada" });
                }

                return Ok(new { success = $"Planilha {sheet.Name} deletada com sucesso!" });
            }
            catch (Exception e)
            {

                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpGet("{id}/finance")]
        public async Task<IActionResult> GetPaginatedFinances([FromRoute] Guid id, [FromQuery] GetPaginatedFinancesQueryDTO queryDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var sheet = await _sheetRepository.GetSheetByIdAsync(id, userId);

                if (sheet == null)
                {
                    return NotFound(new { error = "Planilha não encontrada" });
                }

                var paginatedFinances = await _financeRepository.GetPaginatedFinancesAsync(id, queryDTO);

                var finances = paginatedFinances.Select(finance => finance.ToFinanceWithCategoryDTO());

                var financesAmount = await _financeRepository.GetFinancesAmountAsync(id, queryDTO.Title);

                var financesCount = await _financeRepository.GetFinancesCountAsync(id, queryDTO.Title);

                return Ok(
                    new { finances, financesCount, sheetId = sheet.Id, financesAmount }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost("{id}/finance")]
        public async Task<IActionResult> CreateFinance([FromRoute] Guid id, [FromBody] CreateFinanceDTO createFinanceDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var finance = createFinanceDTO.ToFinanceFromCreateFinanceDTO(id);

                if (finance == null)
                {
                    return BadRequest(new { error = "Campo(s) inválido(s)" });
                }

                var isInitialAmountSet = user.IsInitialAmountSet;

                if (!isInitialAmountSet)
                {
                    return BadRequest(new { error = "Saldo inicial ainda não foi definido" });
                }

                var sheet = await _sheetRepository.GetSheetByIdAsync(id, userId);

                if (sheet == null)
                {
                    return NotFound(new { error = "Planilha não encontrada" });
                }

                var financeMonth = int.Parse(createFinanceDTO.Date.Substring(3, 2));
                var financeYear = createFinanceDTO.Date.Substring(6);

                var sheetMonth = sheet.Name.Split("/")[0];
                var sheetYear = sheet.Name.Split("/")[1];

                var monthMap = Utils.MonthMap();


                if (!monthMap[financeMonth].Equals(sheetMonth) || !financeYear.Equals(sheetYear))
                {
                    return BadRequest(new { error = "O mês e ano da finança devem corresponder ao da planilha" });
                }

                //TODO: USER SUBSCRIPTION

                await _financeRepository.CreateAsync(finance);

                var updatedSucceeded = await _sheetRepository.UpdateTotalAmountAndFinancesCountAsync(id, finance);

                if (!updatedSucceeded)
                {
                    return NotFound(new { error = "Planilha não encontrada" });
                }

                user.TotalAmount = finance.Type == "PROFIT" ? user.TotalAmount + finance.Amount : user.TotalAmount - finance.Amount;

                await _userManager.UpdateAsync(user);

                return Ok(finance.ToFinanceDTO());
            }
            catch (Exception e)
            {

                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}