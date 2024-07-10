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

        [HttpPost("help")]
        public async Task<IActionResult> SendMessage([FromBody] NewHelpMessageDTO bodyDTO)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized("Não autorizado");
                }

                var helpMessage = await _helpMessageRepository.CreateAsync(bodyDTO.Message, user);

                //TODO: criar um metodo no repositorio para fazer essa query
                // const lastMessage = await db.helpMessage.findFirst({
                // 	where: {
                // 		userId: user.id,
                // 	},
                // 	orderBy: {
                // 		createdAt: 'desc',
                // 	},
                // });

                // if (lastMessage && lastMessage.createdAt.getTime() + 1000 * 60 * 60 * 24 > Date.now()) {
                // 	return NextResponse.json(
                // 		{
                // 			message: 'Você só pode enviar uma mensagem a cada 24 horas',
                // 			timeLimit: true,
                // 		},
                // 		{ status: 200 }
                // 	);
                // }

                return Ok("Mensagem enviada com sucesso!");

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}