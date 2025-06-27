using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Sessions.DTOs;
using server.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [Route("api/v1/session")]
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
                throw new InvalidOperationException("User ID could not be determined from the token.");
            }
            return userId;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequestDto request) {
            try {
                var ownerId = GetCurrentUserId();
                var session = await _sessionService.CreateSessionFromTemplateAsync(request, ownerId);
                return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, session);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSessions() {
            var ownerId = GetCurrentUserId();
            var sessions = await _sessionService.GetAllSessionsAsync(ownerId);
            return Ok(sessions);
        }

        [HttpGet("template/{templateId:guid}")]
        public async Task<IActionResult> GetSessionsByTemplate(Guid templateId) {
            var ownerId = GetCurrentUserId();
            var sessions = await _sessionService.GetSessionsByTemplateAsync(templateId, ownerId);
            return Ok(sessions);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSessionById(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.GetSessionByIdAsync(id, ownerId);
            return session == null ? NotFound() : Ok(session);
        }

        [HttpGet("{id:guid}/activities")]
        public async Task<IActionResult> GetSessionActivities(Guid id) {
            var ownerId = GetCurrentUserId();
            var activities = await _sessionService.GetSessionActivitiesAsync(id, ownerId);

            if (activities == null) {
                return NotFound(new { message = "Session not found or you are not authorized to view its activities." });
            }

            return Ok(activities);
        }

        [HttpPost("{id:guid}/start")]
        public async Task<IActionResult> StartSession(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.StartSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already active." }) : Ok(session);
        }

        [HttpPost("{id:guid}/stop")]
        public async Task<IActionResult> StopSession(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.StopSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already finished." }) : Ok(session);
        }

        [HttpPost("{id:guid}/next")]
        public async Task<IActionResult> NextActivity(Guid id) {
            var ownerId = GetCurrentUserId();
            var session = await _sessionService.NextActivityAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found, not active, or on the last activity." }) : Ok(session);
        }
    }
}
