using System.Text.Json;
using server.Data;
using server.Models.Activities;
using Microsoft.EntityFrameworkCore;

namespace server.Services
{
    public class ActivityService
    {
        private readonly AppDbContext _context;

        public ActivityService(AppDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Create an activity from a JSON definition and save it to the database asynchronously
        /// </summary>
        /// <param name="activityJson"> JSON definition of the activity </param>
        /// <returns> The created activity </returns>
        public async Task<Activity> CreateActivityFromDefinitionAsync(JsonElement activityJson) {
            var activity = ActivityFactory.CreateActivity(activityJson);

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<List<Activity>> GetAllActivitiesAsync() {
            return await _context.Activities.ToListAsync();
        }

        public async Task<Activity?> GetActivityByIdAsync(Guid activityId) {
            return await _context.Activities.FirstOrDefaultAsync(a => a.ActivityId == activityId);
        }
    }
}
