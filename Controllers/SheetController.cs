using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.Mappers;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FinnanciaCSharp.Models;
using FinnanciaCSharp.Extensions;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/sheet")]
    public class SheetController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISheetRepository _sheetRepository;
        public SheetController(ISheetRepository sheetRepository, UserManager<User> userManager)
        {
            _sheetRepository = sheetRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetSheets()
        {
            try
            {
                var userId = User.GetUserId();

                if (userId == null)
                {
                    return Unauthorized("Não autorizado");
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSheetById([FromRoute] Guid id)
        {
            try
            {
                var userId = User.GetUserId();

                if (userId == null)
                {
                    return Unauthorized("Não autorizado!");
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return Unauthorized("Não autorizado!");
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

        [HttpPost]
        public async Task<IActionResult> CreateSheet([FromBody] CreateSheetDTO createSheetDTO)
        {
            var currentDate = DateTime.Now;

            var year = createSheetDTO.Year;
            var month = createSheetDTO.Month;

            if (year < currentDate.Year || year > currentDate.Year + 1 || month < 1 || month > 12)
            {
                return BadRequest("Campo(s) inválido(s)");
            }

            try
            {
                var userId = User.GetUserId();

                if (userId == null)
                {
                    return Unauthorized("Não autorizado");
                }

                var sheetExists = await _sheetRepository.SheetExistsByMonthAndYear(month, year, userId);

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
    }
}