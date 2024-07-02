using FinnanciaCSharp.Data;
using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.Mappers;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Enums;

namespace FinnanciaCSharp.Controllers
{
    [ApiController]
    [Route("api/sheet")]
    public class SheetController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public SheetController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetSheet([FromRoute] Guid id)
        {
            var sheet = _context.Sheets.Find(id);

            if (sheet == null)
            {
                return NotFound();
            }

            return Ok(sheet.ToSheetDTOFromSheet());
        }

        [HttpPost]
        public IActionResult CreateSheet([FromBody] CreateSheetDTO createSheetDTO)
        {
            var currentDate = DateTime.Now;

            var year = createSheetDTO.Year;
            var month = createSheetDTO.Month;

            if (year < currentDate.Year || year > currentDate.Year + 1 || month < 1 || month > 12)
            {
                return BadRequest("Campo(s) inválido(s)");
            }

            // var existingSheet = _context.Sheets.FirstOrDefault(sheet => sheet.Name.Equals($"{Month[0]}"))

            // _context.Sheets.Add(newSheet);
            // _context.SaveChanges();

            // return CreatedAtAction(nameof(GetSheet), new { id = newSheet.Id }, newSheet.ToSheetDTOFromSheet());

            return Ok();
        }
    }
}