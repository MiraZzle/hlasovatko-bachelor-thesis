using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Sessions.DTOs;
using server.Services;
using System.Security.Claims;
using server.Extensions;

namespace server.Controllers
{
    [Route("api/v1/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService) {
            _sessionService = sessionService;
        }

        [HttpGet("join/{joinCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSessionByJoinCode(string joinCode) {
            var session = await _sessionService.GetSessionByJoinCodeAsync(joinCode);
            if (session == null)
                return NotFound();
            return Ok(new { id = session.Id, title = session.Title, mode = session.Mode });
        }

        [HttpGet("{sessionId:guid}/state")]
        [AllowAnonymous]
        public async Task<IActionResult> GetParticipantSessionState(Guid sessionId) {
            var state = await _sessionService.GetParticipantSessionStateAsync(sessionId);
            return state == null ? NotFound(new { message = "Session not found or is not active." }) : Ok(state);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> DeleteSession(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var success = await _sessionService.DeleteSessionAsync(id, ownerId);

            if (!success) {
                return NotFound(new { message = "Session not found or you do not have permission to delete it." });
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequestDto request) {
            try {
                var ownerId = this.GetCurrentUserId();
                var session = await _sessionService.CreateSessionFromTemplateAsync(request, ownerId);
                return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, session);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetAllSessions() {
            var ownerId = this.GetCurrentUserId();
            var sessions = await _sessionService.GetAllSessionsAsync(ownerId);
            return Ok(sessions);
        }

        [HttpGet("template/{templateId:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetSessionsByTemplate(Guid templateId) {
            var ownerId = this.GetCurrentUserId();
            var sessions = await _sessionService.GetSessionsByTemplateAsync(templateId, ownerId);
            return Ok(sessions);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSessionById(Guid id) {
            var session = await _sessionService.GetSessionByIdAsync(id);
            return session == null ? NotFound() : Ok(session);
        }

        [HttpGet("{id:guid}/activities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSessionActivities(Guid id) {
            var activities = await _sessionService.GetSessionActivitiesAsync(id);

            if (activities == null) {
                return NotFound(new { message = "Session not found or you are not authorized to view its activities." });
            }

            return Ok(activities);
        }

        [HttpPost("{id:guid}/start")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> StartSession(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var session = await _sessionService.StartSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already active." }) : Ok(session);
        }

        [HttpPost("{id:guid}/stop")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> StopSession(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var session = await _sessionService.StopSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already finished." }) : Ok(session);
        }

        [HttpPost("{id:guid}/next")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> NextActivity(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var session = await _sessionService.NextActivityAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found, not active, or on the last activity." }) : Ok(session);
        }
    }
}
