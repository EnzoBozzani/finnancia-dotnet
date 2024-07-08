using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinnanciaCSharp.Lib
{
    public static class Utils
    {
        private static readonly List<string> FinanceType = new List<string> { "EXPENSE", "PROFIT" };
        public static Dictionary<int, string> MonthMap()
        {
            return new Dictionary<int, string>(){
                { 1, "Janeiro"},
                { 2, "Fevereiro"},
                { 3, "Março"},
                { 4, "Abril"},
                { 5, "Maio"},
                { 6, "Junho"},
                { 7, "Julho"},
                { 8, "Agosto"},
                { 9, "Setembro"},
                { 10, "Outubro"},
                { 11, "Novembro"},
                { 12, "Dezembro"}
            };
        }

        public static bool IsValidFinanceType(string value)
        {
            return FinanceType.Contains(value);
        }
    }
}