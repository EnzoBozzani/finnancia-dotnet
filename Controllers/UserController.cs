using FinnanciaCSharp.DTOs.User;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.Interfaces;
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
        private readonly IHelpMessageRepository _helpMessageRepository;
        public UserController(UserManager<User> userManager, IHelpMessageRepository helpMessageRepository)
        {
            _userManager = userManager;
            _helpMessageRepository = helpMessageRepository;
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
                    return Unauthorized(new { error = "Não autorizado" });
                }

                if (!user.IsInitialAmountSet)
                {
                    return BadRequest(new { error = "O saldo inicial não foi definido" });
                }

                user.TotalAmount = bodyDTO.Amount;

                await _userManager.UpdateAsync(user);

                return Ok(new { success = "Saldo alterado com sucesso" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
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
                    return Unauthorized(new { error = "Não autorizado" });
                }

                if (user.IsInitialAmountSet)
                {
                    return BadRequest(new { error = "Saldo inicial já foi definido" });
                }

                user.IsInitialAmountSet = true;
                user.TotalAmount = bodyDTO.Amount;

                await _userManager.UpdateAsync(user);

                return Ok(new { success = "Saldo inicial definido com sucesso" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost("help")]
        public async Task<IActionResult> SendMessage([FromBody] NewHelpMessageDTO bodyDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var lastMessage = await _helpMessageRepository.GetLastMessage(userId);

                if (lastMessage != null && lastMessage.CreatedAt.AddDays(1) > DateTime.Now)
                {
                    return Ok(new { message = "Você só pode enviar uma mensagem a cada 24 horas", timeLimit = true });
                }

                var helpMessage = await _helpMessageRepository.CreateAsync(bodyDTO.Message, user);

                return Ok(new { success = "Mensagem enviada com sucesso!" });

            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}