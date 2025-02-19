using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using server.Services;

namespace server.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionController(SessionService sessionService) {
            _sessionService = sessionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSession([FromBody] JsonElement sessionDefinition) {
            try {
                var session = await _sessionService.CreateSessionAsync(sessionDefinition);
                return Ok(new { message = "Session created successfully!", sessionId = session.SessionId });
            }
            catch (Exception ex) {
                return BadRequest($"Error processing session: {ex.Message}");
            }
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(Guid sessionId) {
            try {
                var session = await _sessionService.GetSessionAsync(sessionId);
                // write out serialized session object
                Console.WriteLine(
                    JsonSerializer.Serialize(session, new JsonSerializerOptions { WriteIndented = true }));
                if (session == null) {
                    return NotFound();
                }
                return Ok(session);
            }
            catch (Exception ex) {
                return BadRequest($"Error retrieving session: {ex.Message}");
            }
        }
    }
}
