using ChatApp.Application.DTOs.PublicLogin;
using ChatApp.Application.DTOs.PublicRegister;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly IUserRepository _userRepository;
            private readonly ITokenService _tokenService;
        public AuthController(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register_DTO(RegisterDTO request)
            {
            var existingUser = await _userRepository.GetByUsernameAsync(request.RegisterName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User already taken!" });
            }
           
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.RegisterPassword);

            var user = new User()
            {
                UserName = request.RegisterName,
                Password = hashedPassword,
                Email = request.RegisterEmail
            };
            return Ok(new { message = "New User Create" });

        }

            [HttpPost("login")]
            public async Task<IActionResult> Login_DTO(LoginDTO request)
            {
            var user = await _userRepository.GetByUsernameAsync(request.LoginEmail);
            if (user == null)
            {
                return BadRequest(new { message = "The User is incorect" });
            }
            bool PasswordCheck = BCrypt.Net.BCrypt.Verify(request.LoginPassword, user.Password);
            if ( PasswordCheck== null)
            {
                return BadRequest(new { message = "The Password is incorect" });
            }
            var generatedToken = _tokenService.GenerateToken(user);

           return Ok(new { token = generatedToken });

            }
        }
    
}
