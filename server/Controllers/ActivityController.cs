using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Activities.DTOs;
using server.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [Route("api/v1/activity-bank")]
    [ApiController]
    [Authorize(Policy = "AuthenticatedUser")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService) {
            _activityService = activityService;
        }

        private Guid GetCurrentUserId() {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) {
                throw new InvalidOperationException("User ID could not be determined from token.");
            }
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivityBank() {
            var ownerId = GetCurrentUserId();
            var activities = await _activityService.GetBankAsync(ownerId);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBank([FromBody] ActivityBankRequestDto request) {
            try {
                var ownerId = GetCurrentUserId();
                var newActivity = await _activityService.AddToBankAsync(request, ownerId);
                return CreatedAtAction(nameof(GetActivityBank), new { id = newActivity.Id }, newActivity);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
