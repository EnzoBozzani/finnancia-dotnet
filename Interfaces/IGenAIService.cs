using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinnanciaCSharp.Interfaces
{
    public interface IGenAIService
    {
        Task<object?> ChatWithAIAsync();
    }
}