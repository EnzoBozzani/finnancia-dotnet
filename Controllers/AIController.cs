using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.DTOs.GenAI;
using FinnanciaCSharp.Mappers;

namespace FinnanciaCSharp.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenAIService _genAiService;
        private readonly IMessageRepository _messageRepository;
        public AIController(UserManager<User> userManager, IGenAIService genAIService, IMessageRepository messageRepository)
        {
            _userManager = userManager;
            _genAiService = genAIService;
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateAIResponse([FromBody] AiPromptDTO bodyDTO)
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

                var oldMessages = await _messageRepository.GetMessagesAsync(userId);

                int numberOfUserMessages = oldMessages.Count() / 2;

                //verify user subs

                var aiResponse = await _genAiService.ChatWithAIAsync(bodyDTO.Message, oldMessages, user.Name);

                if (aiResponse == null)
                {
                    return StatusCode(500, new { error = "Algo deu errado ao gerar o conteúdo" });
                }

                var userMessage = (await _messageRepository.CreateAsync(bodyDTO.Message, userId, "USER")).ToMessageDTO();
                var modelMessage = (await _messageRepository.CreateAsync(aiResponse, userId, "MODEL")).ToMessageDTO();

                return Ok(new { userMessage, modelMessage });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}