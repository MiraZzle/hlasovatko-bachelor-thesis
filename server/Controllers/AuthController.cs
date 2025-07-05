using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Auth.DTOs;
using server.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using server.Models.Auth;
using server.Extensions;

namespace server.Controllers
{
    /// <summary>
    /// Controller for handling user authentication and registration.
    /// </summary>
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <returns>
        /// 201 Created with the new user ID, name, email if succsful.<br/>
        /// 400 Bad Request if registration fails or input is invalid.
        /// </returns>
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

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <returns>
        /// 200 OK with user info and JWT token if authentication succeeds.<br/>
        /// 400 Bad Request if input is invalid.<br/>
        /// 401 Unauthorized if authentication fails.
        /// </returns>
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
        /// <summary>
        /// Changes the password for authenticated user.
        /// </summary>
        /// <returns>
        /// 200 OK if the password was changed successfully.<br/>
        /// 400 Bad Request if the change fails or input is invalid.
        /// </returns>
        [HttpPost("change-password")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var userId = this.GetCurrentUserId();
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

        /// <summary>
        /// Allows a participant to join a session by providing a session id and join code.
        /// </summary>
        /// <param name="request">The request containing the session id and join code.</param>
        /// <returns>A short-lived JWT for the participant if the credentials are valid.</returns>
        [HttpPost("participant/join")]
        [AllowAnonymous]
        public async Task<IActionResult> JoinSession([FromBody] ValidateParticipantRequestDto request) {
            var result = await _authService.VerifyParticipantAsync(request);

            if (result == null) {
                return Unauthorized("Invalid session ID or join code.");
            }

            return Ok(result);
        }

    }
}
