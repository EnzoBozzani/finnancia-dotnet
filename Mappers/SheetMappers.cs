using FinnanciaCSharp.Lib;
using FinnanciaCSharp.DTOs;
using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Mappers
{
    public static class SheetMappers
    {
        public static SheetDTO ToSheetDTOFromSheet(this Sheet sheet)
        {
            return new SheetDTO
            {
                Id = sheet.Id,
                Name = sheet.Name,
                UserId = sheet.UserId,
                TotalAmount = sheet.TotalAmount,
                Order = sheet.Order,
                FinancesCount = sheet.FinancesCount
            };
        }

        public static Sheet ToSheetFromCreateSheetDTO(this CreateSheetDTO createSheetDTO)
        {
            var monthName = Utils.MonthMap()[createSheetDTO.Month];
            var order = Utils.MonthMap().FirstOrDefault(x => x.Value == monthName).Key;

            return new Sheet
            {
                Name = $"{monthName}/{createSheetDTO.Year}",
                UserId = createSheetDTO.UserId,
                TotalAmount = 0,
                Order = order,
                FinancesCount = 0
            };
        }
    }
}