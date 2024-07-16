using FinnanciaCSharp.Lib;
using FinnanciaCSharp.DTOs;
using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.Models;
using FinnanciaCSharp.DTOs.Finance;

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

        public static Sheet ToSheetFromCreateSheetDTO(this CreateSheetDTO createSheetDTO, string userId)
        {
            var monthName = Utils.MonthMap()[createSheetDTO.Month];
            var order = Utils.MonthMap().FirstOrDefault(x => x.Value == monthName).Key;

            return new Sheet
            {
                Name = $"{monthName}/{createSheetDTO.Year}",
                UserId = userId,
                TotalAmount = 0,
                Order = order,
                FinancesCount = 0
            };
        }

        public static SheetWithFinanceDTO ToSheetWithFinanceDTO(this Sheet sheet)
        {
            var finances = sheet.Finances.Select(finance => finance.ToFinanceDTO());

            return new SheetWithFinanceDTO
            {
                Id = sheet.Id,
                Name = sheet.Name,
                Order = sheet.Order,
                TotalAmount = sheet.TotalAmount,
                FinancesCount = sheet.FinancesCount,
                UserId = sheet.UserId,
                Finances = finances
            };
        }

        public static SheetWithFinanceWithCategoryDTO ToSheetWithFinanceWithCategoryDTO(this Sheet sheet)
        {
            var finances = sheet.Finances.Select(finance => finance.ToFinanceWithCategoryDTO());

            return new SheetWithFinanceWithCategoryDTO
            {
                Id = sheet.Id,
                Name = sheet.Name,
                Order = sheet.Order,
                TotalAmount = sheet.TotalAmount,
                FinancesCount = sheet.FinancesCount,
                UserId = sheet.UserId,
                Finances = finances
            };
        }
    }
}