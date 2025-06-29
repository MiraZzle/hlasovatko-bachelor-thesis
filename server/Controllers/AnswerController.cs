using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Extensions;
using server.Models.Answers.DTOs;
using server.Services;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/v1/answer")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService) {
            _answerService = answerService;
        }


        [HttpPost("{sessionId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitAnswer(Guid sessionId, [FromBody] CreateAnswerRequestDto request) {
            try {
                var result = await _answerService.CreateAnswerAsync(sessionId, request);
                if (result == null) {
                    return BadRequest(new { message = "Answer submission failed. The session may not be active or the activity ID is invalid." });
                }
                return Ok(result);
            }
            catch (JsonException jsonEx) {
                return BadRequest(new { message = $"Invalid answer format: {jsonEx.Message}" });
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("session/{sessionId:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetAllSessionAnswers(Guid sessionId) {
            var ownerId = this.GetCurrentUserId();
            var answers = await _answerService.GetAllAnswersForSessionAsync(sessionId, ownerId);
            return Ok(answers);
        }

        [HttpGet("session/{sessionId:guid}/activity/{activityId:guid}")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetActivityAnswers(Guid sessionId, Guid activityId) {
            var ownerId = this.GetCurrentUserId();
            var answers = await _answerService.GetAnswersForActivityAsync(sessionId, activityId, ownerId);
            return Ok(answers);
        }

        [HttpGet("session/{sessionId:guid}/results")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetSessionResults(Guid sessionId) {
            var ownerId = this.GetCurrentUserId();
            var results = await _answerService.GetAggregatedResultsForSessionAsync(sessionId, ownerId);
            return Ok(results);
        }

        [HttpGet("session/{sessionId:guid}/activity/{activityId:guid}/results")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActivityResults(Guid sessionId, Guid activityId) {
            var result = await _answerService.GetAggregatedResultsForActivityAsync(sessionId, activityId);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
