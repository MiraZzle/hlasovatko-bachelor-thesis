using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models.Activities.DTOs;
using server.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "AuthenticatedUser")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService) {
            _activityService = activityService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody] ActivityRequestDto request) {
            try {
                var newActivity = await _activityService.CreateActivityAsync(request);
                return CreatedAtAction(nameof(GetActivity), new { id = newActivity.Id }, newActivity);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetActivity(Guid id) {
            var activity = await _activityService.GetActivityByIdAsync(id);
            return activity == null ? NotFound() : Ok(activity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivities() {
            var activities = await _activityService.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateActivity(Guid id, [FromBody] ActivityRequestDto request) {
            try {
                var updatedActivity = await _activityService.UpdateActivityAsync(id, request);
                return updatedActivity == null ? NotFound() : Ok(updatedActivity);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteActivity(Guid id) {
            var success = await _activityService.DeleteActivityAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
