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

        public async Task<bool> SheetExistsByMonthAndYearAsync(int month, int year, string userId)
        {
            var MonthMap = Utils.MonthMap();
            return await _context.Sheets
                .AnyAsync(sheet => sheet.Name.Equals($"{MonthMap[month]}/{year}") && sheet.UserId == userId);
        }

        public async Task<List<SheetDTO>> GetSheetsByUserIdAsync(string userId)
        {
            return await _context.Sheets
                .Where(sheet => sheet.UserId == userId)
                .OrderBy(sheet => sheet.Order)
                .Select(sheet => sheet.ToSheetDTOFromSheet())
                .ToListAsync();
        }

        public async Task<Sheet?> DeleteSheetAsync(Guid id)
        {
            var sheet = await _context.Sheets.FindAsync(id);

            if (sheet == null)
            {
                return null;
            }

            _context.Sheets.Remove(sheet);
            await _context.SaveChangesAsync();

            return sheet;
        }

        public async Task<bool> UpdateTotalAmount(Guid sheetId, Finance finance)
        {
            var sheet = await _context.Sheets.FindAsync(sheetId);

            if (sheet == null)
            {
                return false;
            }

            sheet.TotalAmount = finance.Type == "PROFIT" ? sheet.TotalAmount + finance.Amount : sheet.TotalAmount - finance.Amount;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}