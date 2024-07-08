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
                    return Unauthorized("Não autorizado");
                }

                var sheets = await _sheetRepository.GetSheetsByUserIdAsync(userId);

                return Ok(new { sheets = sheets, isInitialAmountSet = user.IsInitialAmountSet });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
                return BadRequest("O ano deve ser entre o ano atual ou o próximo");
            }

            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized("Não autorizado");
                }

                var sheetExists = await _sheetRepository.SheetExistsByMonthAndYearAsync(month, year, userId);

                if (sheetExists)
                {
                    return BadRequest("Planilha já existente");
                }

                var newSheet = createSheetDTO.ToSheetFromCreateSheetDTO(userId);

                await _sheetRepository.CreateAsync(newSheet);

                return CreatedAtAction(nameof(GetSheetById), new { id = newSheet.Id }, newSheet.ToSheetDTOFromSheet());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
                    return Unauthorized("Não autorizado");
                }

                var sheet = await _sheetRepository.GetSheetByIdAsync(id);

                if (sheet == null)
                {
                    return NotFound("Planilha não encontrada!");
                }

                return Ok(sheet.ToSheetDTOFromSheet());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
                    return Unauthorized("Não autorizado");
                }

                var sheet = await _sheetRepository.DeleteSheetAsync(id);

                if (sheet == null)
                {
                    return NotFound("Planilha não encontrada");
                }

                return Ok($"Planilha {sheet.Name} deletada com sucesso!");
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
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
                    return Unauthorized("Não autorizado");
                }

                var finance = createFinanceDTO.ToFinanceFromCreateFinanceDTO(id);

                if (finance == null)
                {
                    return BadRequest("Campo(s) inválido(s)");
                }

                var userTotalAmount = user.TotalAmount;
                var isInitialAmountSet = user.IsInitialAmountSet;

                if (!isInitialAmountSet)
                {
                    return BadRequest("Saldo inicial ainda não foi definido");
                }

                var sheet = await _sheetRepository.GetSheetByIdAsync(id);

                if (sheet == null)
                {
                    return NotFound("Planilha não encontrada");
                }

                var financeMonth = int.Parse(createFinanceDTO.Date.Substring(3, 2));
                var financeYear = createFinanceDTO.Date.Substring(6);

                var sheetMonth = sheet.Name.Split("/")[0];
                var sheetYear = sheet.Name.Split("/")[1];

                var monthMap = Utils.MonthMap();


                if (!monthMap[financeMonth].Equals(sheetMonth) || !financeYear.Equals(sheetYear))
                {
                    return BadRequest("O mês e ano da finança devem corresponder ao da planilha");
                }

                //TODO: USER SUBSCRIPTION

                await _financeRepository.CreateAsync(finance);

                //TODO: totalAmounts: criar novo metodo no sheetRepository e criar um userRepository

                return Ok(finance.ToFinanceDTO());
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }
    }
}