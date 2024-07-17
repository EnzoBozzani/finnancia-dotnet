using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;
using FinnanciaCSharp.DTOs.GenAI;
using FinnanciaCSharp.Mappers;
using Microsoft.AspNetCore.Authorization;
using FinnanciaCSharp.Constants;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenAIService _genAiService;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserSubscriptionRepository _userSubRepository;
        public AIController(UserManager<User> userManager, IGenAIService genAIService, IMessageRepository messageRepository, IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userManager = userManager;
            _genAiService = genAIService;
            _messageRepository = messageRepository;
            _userSubRepository = userSubscriptionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var messages = (await _messageRepository.GetMessagesAsync(userId)).Select(message => message.ToMessageDTO());

                return Ok(messages);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
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

                var userSubscription = await _userSubRepository.GetUserSubscriptionAsync(userId);

                var oldMessages = await _messageRepository.GetMessagesAsync(userId);

                int numberOfUserMessages = oldMessages.Count() / 2;

                if (numberOfUserMessages >= Constants.Constants.MAX_PROMPTS_FOR_FREE && (userSubscription == null || !userSubscription.IsActive))
                {
                    return Ok(new { maxPromptsReached = true });
                }

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