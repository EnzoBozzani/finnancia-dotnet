using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinnanciaCSharp.DTOs.Finance;

namespace FinnanciaCSharp.DTOs.Sheet
{
    public class SheetWithFinanceWithCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int Order { get; set; }
        public int FinancesCount { get; set; } = 0;
        public IEnumerable<FinanceWithCategoryDTO> Finances { get; set; } = new List<FinanceWithCategoryDTO>();
    }
}