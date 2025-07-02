using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Sessions.DTOs;
using server.Services;
using System.Security.Claims;
using server.Extensions;

namespace server.Controllers
{
    /// <summary>
    /// Controller for managing sessions.
    /// </summary>
    [Route("api/v1/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService) {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Gets session details by join code.
        /// </summary>
        /// <param name="joinCode">The join code for the session.</param>
        /// <returns>
        /// 200 OK with session id, title, and mode if found.<br/>
        /// 404 Not Found if no session matches the join code.
        /// </returns>
        [HttpGet("join/{joinCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSessionByJoinCode(string joinCode) {
            var session = await _sessionService.GetSessionByJoinCodeAsync(joinCode);
            if (session == null)
                return NotFound();
            return Ok(new { id = session.Id, title = session.Title, mode = session.Mode });
        }

        /// <summary>
        /// Gets the current state of a session for a participant.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>
        /// 200 OK with the session state.<br/>
        /// 404 Not Found if the session does not exist or is not active.
        /// </returns>
        [HttpGet("{sessionId:guid}/state")]
        [AllowAnonymous]
        public async Task<IActionResult> GetParticipantSessionState(Guid sessionId) {
            var state = await _sessionService.GetParticipantSessionStateAsync(sessionId);
            return state == null ? NotFound(new { message = "Session not found or not active." }) : Ok(state);
        }

        /// <summary>
        /// Deletes a session owned by the current user.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// 204 No Content if deleted.<br/>
        /// 404 Not Found if the session does not exist or the user lacks the permission.
        /// </returns>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> DeleteSession(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var success = await _sessionService.DeleteSessionAsync(id, ownerId);

            if (!success) {
                return NotFound(new { message = "Session not found or lacking permission." });
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new session from a template.
        /// </summary>
        /// <param name="request">The session creation details.</param>
        /// <returns>
        /// 201 Created with the new session details.<br/>
        /// 400 Bad Request if creation fails.
        /// </returns>
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

        /// <summary>
        /// Gets all sessions owned by the current user.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of sessions.
        /// </returns>
        [HttpGet]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetAllSessions() {
            var ownerId = this.GetCurrentUserId();
            var sessions = await _sessionService.GetAllSessionsAsync(ownerId);
            return Ok(sessions);
        }

        /// <summary>
        /// Gets all sessions created from a specific template for the current user.
        /// </summary>
        /// <param name="templateId">The template id.</param>
        /// <returns>
        /// 200 OK with a list of sessions.
        /// </returns>
        [HttpGet("template/{templateId:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetSessionsByTemplate(Guid templateId) {
            var ownerId = this.GetCurrentUserId();
            var sessions = await _sessionService.GetSessionsByTemplateAsync(templateId, ownerId);
            return Ok(sessions);
        }

        /// <summary>
        /// Gets session details by session ID.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// 200 OK with session details.<br/>
        /// 404 Not Found if the session does not exist.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSessionById(Guid id) {
            var session = await _sessionService.GetSessionByIdAsync(id);
            return session == null ? NotFound() : Ok(session);
        }

        /// <summary>
        /// Gets all activities for a session.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// 200 OK with a list of activities.<br/>
        /// 404 Not Found if the session does not exist or is not accessible.
        /// </returns>
        [HttpGet("{id:guid}/activities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSessionActivities(Guid id) {
            var activities = await _sessionService.GetSessionActivitiesAsync(id);

            if (activities == null) {
                return NotFound(new { message = "Session not found or lacking permission." });
            }

            return Ok(activities);
        }

        /// <summary>
        /// Starts a session owned by the current user.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// 200 OK with updated session details.<br/>
        /// 404 Not Found if the session does not exist or is already active.
        /// </returns>
        [HttpPost("{id:guid}/start")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> StartSession(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var session = await _sessionService.StartSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already active." }) : Ok(session);
        }

        /// <summary>
        /// Stops a session owned by the current user.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// 200 OK with updated session details.<br/>
        /// 404 Not Found if the session does not exist or is already finished.
        /// </returns>
        [HttpPost("{id:guid}/stop")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> StopSession(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var session = await _sessionService.StopSessionAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found or already finished." }) : Ok(session);
        }

        /// <summary>
        /// Advances the session to the next activity.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// 200 OK with updated session details.<br/>
        /// 404 Not Found if the session does not exist, is not active, or is on the last activity.
        /// </returns>
        [HttpPost("{id:guid}/next")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> NextActivity(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var session = await _sessionService.NextActivityAsync(id, ownerId);
            return session == null ? NotFound(new { message = "Session not found / active / currently on last activity" }) : Ok(session);
        }
    }
}
