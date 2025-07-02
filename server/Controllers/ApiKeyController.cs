using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using server.Extensions;

namespace server.Controllers
{
    /// <summary>
    /// Controller for managing API keys for users.
    /// </summary>
    [Route("api/v1/apikey")]
    [ApiController]
    [Authorize]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService) {
            _apiKeyService = apiKeyService;
        }

        /// <summary>
        /// Retrieves a partial key for the current user.
        /// </summary>
        /// <returns>
        /// 200 OK with apikeydto if a key exists.<br/>
        /// 404 Not Found if the user does not have an API key.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetKey() {
            var userId = this.GetCurrentUserId();
            var keyInfo = await _apiKeyService.GetKeyForUserAsync(userId);

            if (keyInfo == null) {
                return NotFound(new { message = "No API Key found for this user." });
            }

            return Ok(keyInfo);
        }

        /// <summary>
        /// Regenerates the API key for the current user.
        /// </summary>
        /// <returns>
        /// 200 OK with apikeydto containing the new RAW!!! API key and key info.
        /// </returns>
        [HttpPost("regenerate")]
        public async Task<IActionResult> RegenerateKey() {
            var userId = this.GetCurrentUserId();
            var newKey = await _apiKeyService.RegenerateKeyAsync(userId);

            return Ok(newKey);
        }
    }
}
