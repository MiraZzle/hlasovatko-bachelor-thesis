using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using server.Extensions;

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

        [HttpGet]
        public async Task<IActionResult> GetKey() {
            var userId = this.GetCurrentUserId();
            var keyInfo = await _apiKeyService.GetKeyForUserAsync(userId);

            if (keyInfo == null) {
                return NotFound(new { message = "No API Key found for this user." });
            }

            return Ok(keyInfo);
        }

        [HttpPost("regenerate")]
        public async Task<IActionResult> RegenerateKey() {
            var userId = this.GetCurrentUserId();
            var newKey = await _apiKeyService.RegenerateKeyAsync(userId);

            return Ok(newKey);
        }
    }
}
