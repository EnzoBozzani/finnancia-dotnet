using FinnanciaCSharp.DTOs.Finance;
using FinnanciaCSharp.Lib;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Mappers
{
    public static class FinanceMappers
    {
        public static Finance? ToFinanceFromCreateFinanceDTO(this CreateFinanceDTO createFinanceDTO, Guid sheetId)
        {
            var convertSucceeded = int.TryParse(createFinanceDTO.Date.Substring(0, 2), out var order);

            if (!convertSucceeded)
            {
                return null;
            }

            if (!Utils.IsValidFinanceType(createFinanceDTO.FinanceType))
            {
                return null;
            }

            return new Finance
            {
                Title = createFinanceDTO.Title,
                Amount = createFinanceDTO.Amount,
                Date = createFinanceDTO.Date,
                Order = order,
                Type = createFinanceDTO.FinanceType,
                SheetId = sheetId,
                CategoryId = createFinanceDTO.CategoryId
            };
        }

        public static FinanceDTO ToFinanceDTO(this Finance finance)
        {
            return new FinanceDTO
            {
                Id = finance.Id,
                Title = finance.Title,
                Amount = finance.Amount,
                Date = finance.Date,
                Order = finance.Order,
                Type = finance.Type,
                SheetId = finance.SheetId,
                CategoryId = finance.CategoryId,
                CreatedAt = finance.CreatedAt,
                UpdatedAt = finance.UpdatedAt
            };
        }
    }
}