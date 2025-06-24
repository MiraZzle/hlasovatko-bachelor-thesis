using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/v1/apikey")]
    [ApiController]
    [Authorize]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService) {
            _apiKeyService = apiKeyService;
        }

        private Guid GetCurrentUserId() {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId)) {
                throw new InvalidOperationException("User ID not found in token.");
            }
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetKey() {
            var userId = GetCurrentUserId();
            var keyInfo = await _apiKeyService.GetKeyForUserAsync(userId);

            if (keyInfo == null) {
                return NotFound(new { message = "No API Key found for this user." });
            }

            return Ok(keyInfo);
        }

        [HttpPost("regenerate")]
        public async Task<IActionResult> RegenerateKey() {
            var userId = GetCurrentUserId();
            var newKey = await _apiKeyService.RegenerateKeyAsync(userId);

            return Ok(newKey);
        }
    }
}
