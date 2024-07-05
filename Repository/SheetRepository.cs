using FinnanciaCSharp.Data;
using FinnanciaCSharp.DTOs;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Lib;
using FinnanciaCSharp.Mappers;
using FinnanciaCSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Repository
{
    public class SheetRepository : ISheetRepository
    {
        private readonly ApplicationDBContext _context;
        public SheetRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Sheet> CreateAsync(Sheet sheet)
        {
            await _context.Sheets.AddAsync(sheet);
            await _context.SaveChangesAsync();

            return sheet;
        }

        public async Task<Sheet?> GetSheetByIdAsync(Guid id)
        {
            return await _context.Sheets.FirstOrDefaultAsync(sheet => sheet.Id == id);
        }

        public async Task<bool> SheetExistsByMonthAndYear(int month, int year, Guid userId)
        {
            var MonthMap = Utils.MonthMap();
            return await _context.Sheets
                .AnyAsync(sheet => sheet.Name.Equals($"{MonthMap[month]}/{year}") && sheet.UserId == userId);
        }

        public async Task<List<SheetDTO>> GetSheetsByUserIdAsync(Guid userId)
        {
            return await _context.Sheets
                .Where(sheet => sheet.UserId == userId)
                .Select(sheet => sheet.ToSheetDTOFromSheet())
                .ToListAsync();
        }
    }
}