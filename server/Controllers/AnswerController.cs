using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using server.Services;
using server.Models.Answers;

namespace server.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly AnswerService _answerService;

        public AnswerController(AnswerService answerService) {
            _answerService = answerService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAnswer([FromBody] JsonElement answerJson) {
            try {
                bool success = await _answerService.SubmitAnswerAsync(answerJson);
                if (!success)
                    return BadRequest("Failed to submit answer.");

                return Ok(new { message = "Answer submitted successfully!" });
            }
            catch (Exception ex) {
                Console.WriteLine($"Err with answer: {ex.Message}");
                return BadRequest($"Error submitting answer: {ex.Message}");
            }
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetSessionAnswers(Guid sessionId) {
            try {
                var answers = await _answerService.GetSessionAnswersAsync(sessionId);
                if (answers == null || answers.Count == 0)
                    return NotFound("No answers found for this session.");

                return Ok(answers);
            }
            catch (Exception ex) {
                return BadRequest($"Error retrieving answers: {ex.Message}");
            }
        }

        [HttpGet("activity/{activityId}")]
        public async Task<IActionResult> GetActivityAnswers(Guid activityId) {
            try {
                var answers = await _answerService.GetActivityAnswersAsync(activityId);
                if (answers == null || answers.Count == 0)
                    return NotFound("No answers found for this activity.");

                return Ok(answers);
            }
            catch (Exception ex) {
                return BadRequest($"Error retrieving answers: {ex.Message}");
            }
        }
    }
}
