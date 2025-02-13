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
    }
}
