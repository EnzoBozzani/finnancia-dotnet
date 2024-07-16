using System.Security.Claims;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        }
    }
}