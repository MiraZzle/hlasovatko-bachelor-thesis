using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Extensions;
using server.Models.Sessions.DTOs;
using server.Services;
using System.Security.Claims;

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
        private readonly AppDbContext _context;

        public SessionController(ISessionService sessionService, AppDbContext context) {
            _sessionService = sessionService;
            _context = context;
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
        /// Gets the current state of a session for a participant. Requires a valid Participant token.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>
        /// 200 OK with the session state.<br/>
        /// 401 Unauthorized if the token is invalid.<br/>
        /// 403 Forbidden if the token does not belong to this session.<br/>
        /// 404 Not Found if the session does not exist or is not active.
        /// </returns>
        [HttpGet("{sessionId:guid}/state")]
        [Authorize(Policy = "Participant")]
        public async Task<IActionResult> GetParticipantSessionState([FromRoute] Guid sessionId) {
            var sessionIdClaim = User.FindFirst("sessionId");

            if (sessionIdClaim == null || !Guid.TryParse(sessionIdClaim.Value, out var sessionIdFromToken)) {
                return Unauthorized("Invalid participant token.");
            }

            // Check if session ids match
            if (sessionId != sessionIdFromToken) {
                return Forbid("You are not authorized to access this session state.");
            }

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
        /// Gets all activities for a session. Requires a valid User or Participant token.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// 200 OK with a list of activities.<br/>
        /// 403 Forbidden if the user is not authorized for this session.<br/>
        /// 404 Not Found if the session does not exist.
        /// </returns>
        [HttpGet("{id:guid}/activities")]
        [Authorize(Policy = "SessionViewer")]
        public async Task<IActionResult> GetSessionActivities(Guid id) {
            // Use the new extension method for cleaner authorization logic.
            var isAuthorized = await this.IsAuthorizedForSessionAsync(id, _context);

            if (!isAuthorized) {
                return Forbid("You are not authorized to view activities for this session.");
            }

            var activities = await _sessionService.GetSessionActivitiesAsync(id);

            if (activities == null) {
                return NotFound(new { message = "Session not found." });
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
