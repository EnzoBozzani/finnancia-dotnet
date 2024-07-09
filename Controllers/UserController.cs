using FinnanciaCSharp.DTOs.User;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut("amount")]
        public async Task<IActionResult> ChangeUserAmount([FromBody] SetAmountDTO bodyDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized("Não autorizado");
                }

                if (!user.IsInitialAmountSet)
                {
                    return BadRequest("O saldo inicial não foi definido");
                }

                user.TotalAmount = bodyDTO.Amount;

                await _userManager.UpdateAsync(user);

                return Ok("Saldo alterado com sucesso");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("initialAmount")]
        public async Task<IActionResult> SetInitialAmount([FromBody] SetAmountDTO bodyDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized("Não autorizado");
                }

                if (user.IsInitialAmountSet)
                {
                    return BadRequest("Saldo inicial já foi definido");
                }

                user.IsInitialAmountSet = true;
                user.TotalAmount = bodyDTO.Amount;

                await _userManager.UpdateAsync(user);

                return Ok("Saldo inicial definido com sucesso");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}