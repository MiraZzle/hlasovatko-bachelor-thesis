using System.Text.Json;
using server.Data;
using server.Entities;
using server.Models.Activities;
using Microsoft.EntityFrameworkCore;

namespace server.Services
{
    public class SessionService
    {
        private readonly AppDbContext _context;
        private readonly ActivityService _activityService;

        public SessionService(AppDbContext context, ActivityService activityService) {
            _context = context;
            _activityService = activityService;
        }

        public async Task<Session> CreateSessionAsync(JsonElement sessionDefinition) {
            var session = new Session();
            session.InitializeFromJson(sessionDefinition);

            var activitiesJson = sessionDefinition.GetProperty("activities");
            foreach (var activityJson in activitiesJson.EnumerateArray()) {
                var activity = await _activityService.CreateActivityFromDefinitionAsync(activityJson);
                session.activities.Add(activity);
            }

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<Session?> GetSessionAsync(Guid sessionId) {
            return await _context.Sessions
                .Include(s => s.activities)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }
    }
}
