﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Extensions;
using server.Models.Answers.DTOs;
using server.Services;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Controllers
{
    /// <summary>
    /// Controller for handling answer submissions and retrievals.
    /// </summary>
    [Route("api/v1/answer")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly AppDbContext _context;

        public AnswerController(IAnswerService answerService, AppDbContext context) {
            _answerService = answerService;
            _context = context;
        }

        /// <summary>
        /// Submits an answer for a specific session and activity.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="request">>The answer details, including ActivityId, ParticipantId, and AnswerJson.</param>
        /// <returns>
        /// 200 OK with the created answer if successful.<br/>
        /// 400 Bad Request if submission fails or the answer format is invalid.<br/>
        /// 401 Unauthorized if the token is invalid.<br/>
        /// 403 Forbidden if the token does not belong to the specified session.
        /// 500 Internal Server Error for unexpected errors.
        /// </returns>
        [HttpPost("{sessionId:guid}")]
        [Authorize(Policy = "Participant")]
        public async Task<IActionResult> SubmitAnswer([FromRoute] Guid sessionId, [FromBody] CreateAnswerRequestDto request) {
            try {
                var participantIdClaim = User.FindFirst("participantId");
                var sessionIdClaim = User.FindFirst("sessionId");

                // Parse the claims
                if (participantIdClaim == null || sessionIdClaim == null ||
                    !Guid.TryParse(participantIdClaim.Value, out var participantId) ||
                    !Guid.TryParse(sessionIdClaim.Value, out var sessionIdFromToken)) {
                    return Unauthorized("Invalid participant token.");
                }

                // Check if session ids match
                if (sessionId != sessionIdFromToken) {
                    return Forbid("You are not authorized to submit answers for this session.");
                }

                var result = await _answerService.CreateAnswerAsync(sessionId, participantId, request);

                if (result == null) {
                    return BadRequest(new { message = "Answer submission failed. The session may not be active or the activity may be incorrect." });
                }

                return Ok(result);
            }
            catch (JsonException jsonEx) {
                return BadRequest(new { message = $"Invalid answer format: {jsonEx.Message}" });
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "Unexpected error occurred", details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all answers for a specific session for the current user.
        /// Requires the user to be authenticated.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>
        /// 200 OK with a list of answers for the session.
        /// </returns>
        [HttpGet("session/{sessionId:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetAllSessionAnswers(Guid sessionId) {
            var ownerId = this.GetCurrentUserId();
            var answers = await _answerService.GetAllAnswersForSessionAsync(sessionId, ownerId);
            return Ok(answers);
        }

        /// <summary>
        /// Retrieves all answers for a specific activity within a session for the current user.
        /// Requires the user to be authenticated.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="activityId">The activity id.</param>
        /// <returns>
        /// 200 OK with a list of answers for the activity in the session.
        /// </returns>
        [HttpGet("session/{sessionId:guid}/activity/{activityId:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetActivityAnswers(Guid sessionId, Guid activityId) {
            var ownerId = this.GetCurrentUserId();
            var answers = await _answerService.GetAnswersForActivityAsync(sessionId, activityId, ownerId);
            return Ok(answers);
        }

        /// <summary>
        /// Retrieves aggregated results for all activities in a session for the current user.
        /// Requires the user to be authenticated.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>
        /// 200 OK with aggregated results for the session.
        /// </returns>
        [HttpGet("session/{sessionId:guid}/results")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetSessionResults(Guid sessionId) {
            var ownerId = this.GetCurrentUserId();
            var results = await _answerService.GetAggregatedResultsForSessionAsync(sessionId, ownerId);
            return Ok(results);
        }

        /// <summary>
        /// Retrieves aggregated results for a specific activity in a session.
        /// Requires either a valid Participant token for the session or a User token for the session owner.
        /// </summary>
        [HttpGet("session/{sessionId:guid}/activity/{activityId:guid}/results")]
        [Authorize(Policy = "SessionViewer")]
        public async Task<IActionResult> GetActivityResults(Guid sessionId, Guid activityId) {
            var isAuthorized = await this.IsAuthorizedForSessionAsync(sessionId, _context);

            if (!isAuthorized) { return Forbid(); }

            var result = await _answerService.GetAggregatedResultsForActivityAsync(sessionId, activityId);

            return result == null ? NotFound() : Ok(result);
        }
    }
}
