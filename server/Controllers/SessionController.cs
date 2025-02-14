using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using server.Data;
using server.Entities;
using System.Text.Json;

namespace server.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionController : ControllerBase {
        private readonly AppDbContext _context;

        public SessionController(AppDbContext context) {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSession([FromBody] JsonElement sessionDefinition) {
            try {
                var session = new Session();
                session.InitializeFromJson(sessionDefinition);
                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Session created successfully!", sessionId = session.SessionId });
            }
            catch (Exception ex) {
                return BadRequest($"Error processing session: {ex.Message}");
            }
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(Guid sessionId) {
            try {
                var session = await _context.Sessions.FindAsync(sessionId);
                if (session == null) {
                    return NotFound();
                }
                return Ok(session);
            }
            catch (Exception ex) {
                return BadRequest($"Error retrieving session: {ex.Message}");
            }
        }

        [HttpPost("{sessionId}/submit")]
        public async Task<IActionResult> SubmitAnswers(Guid sessionId, [FromBody] JsonElement answers) {
            try {
                var session = await _context.Sessions.FindAsync(sessionId);
                if (session == null) {
                    return NotFound();
                }
                // add logic to process answers
                Console.WriteLine($"Answers received for session {sessionId}: {JsonSerializer.Serialize(answers)}");
                return Ok(new { message = "Answers submitted successfully!" });
            }
            catch (Exception ex) {
                return BadRequest($"Error submitting answers: {ex.Message}");
            }
        }
    }
}
