using FinnanciaCSharp.DTOs.User;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;
        public AuthController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid || loginDTO.Email == null || loginDTO.Password == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

                if (user == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                var result = await _signinManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

                if (!result.Succeeded)
                {
                    return Unauthorized(new { error = "Credenciais inválidas" });
                }

                return Ok(
                    new NewUserDTO
                    {
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid || registerDto.Email == null || registerDto.Password == null || registerDto.Name == null)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var appUser = new User
                {
                    UserName = registerDto.Email,
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDTO
                        {
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        });
                    }

                    return StatusCode(500, new { error = roleResult.Errors });
                }

                return StatusCode(500, new { error = createdUser.Errors });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}