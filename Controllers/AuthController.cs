using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace FinnanciaCSharp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("google")]
        public IActionResult SignInWithGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
                return BadRequest();

            // Extrair informações do token ou claims
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // Retornar ou armazenar informações conforme necessário
            return Ok(new
            {
                Token = accessToken,
                Claims = claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}