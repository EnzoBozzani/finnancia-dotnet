using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.Mappers;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FinnanciaCSharp.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/sheet")]
    public class SheetController : ControllerBase
    {
        private readonly ISheetRepository _sheetRepository;
        public SheetController(ISheetRepository sheetRepository)
        {
            _sheetRepository = sheetRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSheets([FromQuery] Guid userId)
        {
            var sheets = await _sheetRepository.GetSheetsByUserIdAsync(userId);

            Console.WriteLine("oi");

            return Ok(sheets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSheetById([FromRoute] Guid id)
        {
            var sheet = await _sheetRepository.GetSheetByIdAsync(id);

            if (sheet == null)
            {
                return NotFound();
            }

            return Ok(sheet.ToSheetDTOFromSheet());
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
                var sheetExists = await _sheetRepository.SheetExistsByMonthAndYear(month, year, createSheetDTO.UserId);

                if (sheetExists)
                {
                    return BadRequest("Planilha já existente");
                }

                var newSheet = createSheetDTO.ToSheetFromCreateSheetDTO();

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