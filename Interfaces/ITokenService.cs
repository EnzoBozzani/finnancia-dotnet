using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}