using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Templates.DTOs;
using server.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "AuthenticatedUser")]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService) {
            _templateService = templateService;
        }

        private Guid GetCurrentUserId() {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) {
                throw new InvalidOperationException("User ID could not be determined from token");
            }
            return userId;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateRequestDto request) {
            try {
                var ownerId = GetCurrentUserId();
                var newTemplate = await _templateService.CreateTemplateAsync(request, ownerId);
                return CreatedAtAction(nameof(GetTemplate), new { id = newTemplate.Id }, newTemplate);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTemplate(Guid id) {
            // TODO: check for ownership
            var template = await _templateService.GetTemplateByIdAsync(id);
            return template == null ? NotFound() : Ok(template);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTemplatesForUser() {
            var ownerId = GetCurrentUserId();
            var templates = await _templateService.GetAllTemplatesForUserAsync(ownerId);
            return Ok(templates);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTemplate(Guid id) {
            var ownerId = GetCurrentUserId();
            var success = await _templateService.DeleteTemplateAsync(id, ownerId);
            return success ? NoContent() : NotFound(new { message = "Template not found or you do not have permission to delete it" });
        }
    }
}
