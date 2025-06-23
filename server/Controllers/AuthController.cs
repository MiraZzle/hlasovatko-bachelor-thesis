using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Auth.DTOs;
using server.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestDto request) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var newUser = await _authService.RegisterAsync(request);
                return CreatedAtAction(nameof(Register), new { id = newUser.Id }, new { newUser.Id, newUser.Name, newUser.Email });
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto request) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var authResponse = await _authService.LoginAsync(request);
                return Ok(authResponse);
            }
            catch (Exception ex) {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult TestAuth() {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userId == null) {
                return Unauthorized();
            }

            return Ok(new {
                message = "JWT is valid!",
                authenticatedUserId = userId,
                authenticatedUserName = userName,
                authenticatedUserEmail = userEmail
            });
        }
    }
}
