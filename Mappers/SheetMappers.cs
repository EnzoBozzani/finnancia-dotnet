using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}