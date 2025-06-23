using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Sessions.DTOs;
using server.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "AuthenticatedUser")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService) {
            _sessionService = sessionService;
        }

        private Guid GetCurrentUserId() {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) {
                throw new InvalidOperationException("User ID could not be determined from token.");
            }
            return userId;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequestDto request) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.CreateSessionFromTemplateAsync(request, ownerId);
            return CreatedAtAction(nameof(GetSession), new { id = session.Id }, session);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSession(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.GetSessionByIdAsync(id, ownerId);
            return session == null ? NotFound() : Ok(session);
        }

        [HttpPost("{id:guid}/start")]
        public async Task<IActionResult> StartSession(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.StartSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already active" }) : Ok(session);
        }

        [HttpPost("{id:guid}/stop")]
        public async Task<IActionResult> StopSession(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.StopSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already finished" }) : Ok(session);
        }

        [HttpPost("{id:guid}/next")]
        public async Task<IActionResult> NextActivity(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.NextActivityAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or not active" }) : Ok(session);
        }
    }
}
