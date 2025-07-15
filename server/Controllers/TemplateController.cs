using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Templates.DTOs;
using server.Services;
using System.Security.Claims;
using server.Extensions;

namespace server.Controllers
{
    /// <summary>
    /// Controller for managing templates. Requires authenticated users.
    /// Requires authenticated user access.
    /// </summary>
    [Route("api/v1/template")]
    [ApiController]
    [Authorize(Policy = "AuthenticatedUser")]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService) {
            _templateService = templateService;
        }

        /// <summary>
        /// Creates new template for the authenticated user.
        /// </summary>
        /// <param name="request">
        /// The template details.
        /// </param>
        /// <returns>
        /// 201 Created with the new template if successful.<br/>
        /// 400 Bad Request if input is invalid or creation fails.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateRequestDto request) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var ownerId = this.GetCurrentUserId();
                var newTemplate = await _templateService.CreateTemplateAsync(request, ownerId);
                return CreatedAtAction(nameof(GetTemplate), new { id = newTemplate.Id }, newTemplate);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a template by its unique id.
        /// </summary>
        /// <param name="id">The template id.</param>
        /// <returns>
        /// 200 OK with the template details if found.<br/>
        /// 404 Not Found if the template does not exist.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTemplate(Guid id) {
            var template = await _templateService.GetTemplateByIdAsync(id);
            return template == null ? NotFound() : Ok(template);
        }

        /// <summary>
        /// Retrieves all templates owned for the user.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of templates.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTemplatesForUser() {
            var ownerId = this.GetCurrentUserId();
            var templates = await _templateService.GetAllTemplatesForUserAsync(ownerId);
            return Ok(templates);
        }

        /// <summary>
        /// Updates an existing templates definition and settings.
        /// </summary>
        /// <param name="id">The template id.</param>
        /// <param name="request"> The updated template data, including /// </param>
        /// <returns>
        /// 200 OK with the updated template if successful.<br/>
        /// 400 Bad Request if input is invalid.<br/>
        /// 404 Not Found if the template does not exist or user lacks permission.
        /// </returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTemplate(Guid id, [FromBody] UpdateTemplateDto request) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerId = this.GetCurrentUserId();
            var updatedTemplate = await _templateService.UpdateTemplateAsync(id, request, ownerId);

            if (updatedTemplate == null) {
                return NotFound(new { message = "Template not found or you do not have the permission." });
            }
            return Ok(updatedTemplate);
        }

        /// <summary>
        /// Updates only the settings of an existing template.
        /// </summary>
        /// <param name="id">The template id.</param>
        /// <param name="request"> The updated settings. </param>
        /// <returns>
        /// 200 OK with the updated template if successful.<br/>
        /// 400 Bad Request if input is invalid.<br/>
        /// 404 Not Found if the template does not exist or user lacks permission.
        /// </returns>
        [HttpPut("{id:guid}/settings")]
        public async Task<IActionResult> UpdateTemplateSettings(Guid id, [FromBody] UpdateTemplateSettingsDto request) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerId = this.GetCurrentUserId();
            var updatedTemplate = await _templateService.UpdateTemplateSettingsAsync(id, request, ownerId);

            if (updatedTemplate == null) {
                return NotFound(new { message = "Template not found or you do not have the permission." });
            }

            return Ok(updatedTemplate);
        }

        /// <summary>
        /// Deletes a template owned by the authenticated user.
        /// </summary>
        /// <param name="id">The template id.</param>
        /// <returns>
        /// 204 No Content if deleted.<br/>
        /// 404 Not Found if the template does not exist or user lacks permission.
        /// </returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTemplate(Guid id) {
            var ownerId = this.GetCurrentUserId();
            var success = await _templateService.DeleteTemplateAsync(id, ownerId);

            if (!success) {
                return NotFound(new { message = "Template not found or you do not have permission." });
            }

            return NoContent();
        }
    }
}
