using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Auth.DTOs;
using server.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using server.Models.Auth;

namespace server.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        private Guid GetCurrentUserId() {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) {
                throw new InvalidOperationException("User ID could not be determined from token.");
            }
            return userId;
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

        [HttpPost("change-password")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var userId = GetCurrentUserId();
                var success = await _authService.ChangePasswordAsync(userId, request);
                if (success) {
                    return Ok(new { message = "Password changed successfully." });
                }
                return BadRequest(new { message = "Failed to change password." });
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
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
