namespace FinnanciaCSharp.Interfaces
{
    public interface IGenAIService
    {
        Task<string?> ChatWithAIAsync();
    }
}