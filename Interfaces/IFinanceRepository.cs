using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IFinanceRepository
    {
        Task<Finance> CreateAsync(Finance finance);
    }
}