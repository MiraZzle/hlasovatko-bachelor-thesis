using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService) {
            _apiKeyService = apiKeyService;
        }

        /// <summary>
        /// Gets the current user's ID from their JWT claims.
        /// </summary>
        private Guid GetCurrentUserId() {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId)) {
                // This should not happen if the [Authorize] attribute is working correctly
                throw new InvalidOperationException("User ID not found in token.");
            }
            return userId;
        }

        /// <summary>
        /// Gets metadata for the currently authenticated user's API key.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetKey() {
            var userId = GetCurrentUserId();
            var keyInfo = await _apiKeyService.GetKeyForUserAsync(userId);

            if (keyInfo == null) {
                return NotFound(new { message = "No API Key found for this user." });
            }

            return Ok(keyInfo);
        }

        /// <summary>
        /// Generates a new API key for the authenticated user, replacing any old one.
        /// </summary>
        [HttpPost("regenerate")]
        public async Task<IActionResult> RegenerateKey() {
            var userId = GetCurrentUserId();
            var newKey = await _apiKeyService.RegenerateKeyAsync(userId);

            // Returns the full, raw API key for the user to copy.
            return Ok(newKey);
        }
    }
}
