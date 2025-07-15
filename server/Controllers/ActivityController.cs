using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Activities.DTOs;
using server.Services;
using System.Security.Claims;
using server.Extensions;

namespace server.Controllers
{
    /// <summary>
    /// Controller for handling activity bank operations.
    /// Requires authenticated user access.
    /// </summary>
    [Route("api/v1/activity-bank")]
    [ApiController]
    [Authorize(Policy = "AuthenticatedUser")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService) {
            _activityService = activityService;
        }

        /// <summary>
        /// Retrieves the all activities in the activity bank for the current user.
        /// </summary>
        /// <returns> All activities in the bank if succesful. </returns>
        [HttpGet]
        public async Task<IActionResult> GetActivityBank() {
            var ownerId = this.GetCurrentUserId();
            var activities = await _activityService.GetBankAsync(ownerId);
            return Ok(activities);
        }

        /// <summary>
        /// Adds a new activity to the current user's activity bank.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddToBank(
            [FromBody] ActivityRequestDto request) {
            try {
                var ownerId = this.GetCurrentUserId();
                var newActivity = await _activityService.AddToBankAsync(request, ownerId);
                return CreatedAtAction(nameof(GetActivityBank), new { id = newActivity.Id }, newActivity);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
