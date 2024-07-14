using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;

namespace FinnanciaCSharp.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenAIService _genAiService;
        public AIController(UserManager<User> userManager, IGenAIService genAIService)
        {
            _userManager = userManager;
            _genAiService = genAIService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateAIResponse()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                //TODO: User Subscription





                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}