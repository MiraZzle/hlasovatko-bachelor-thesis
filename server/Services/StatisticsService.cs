using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models.Statistics.DTOs;
using server.Services;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Services.Analytics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public StatisticsService(IDbContextFactory<AppDbContext> contextFactory) {
            _contextFactory = contextFactory;
        }

        public async Task<StatisticsDto> GetStatistics(Guid userId) {
            var sessionsCountTask = GetUserSessionsCount(userId);
            var activitiesCountTask = GetUserActivitiesCount(userId);

            var mostCommonActivityTask = GetMostCommonActivityType(userId);

            // Prevent locking the db
            var totalSessions = await GetUserSessionsCount(userId);
            var totalActivities = await GetUserActivitiesCount(userId);
            var mostCommonActivityType = await GetMostCommonActivityType(userId);

            return new StatisticsDto {
                TotalSessions = totalSessions,
                TotalActivities = totalActivities,
                MostCommonActivityType = mostCommonActivityType ?? "N/A"
            };
        }

        public async Task<string> GetStatisticsCsv(Guid userId) {
            var stats = await GetStatistics(userId);
            var builder = new StringBuilder();

            // Define the CSV structure
            builder.AppendLine("Statistic,Value");
            builder.AppendLine($"Total Sessions,{stats.TotalSessions}");
            builder.AppendLine($"Total Activities,{stats.TotalActivities}");
            builder.AppendLine($"Most Common Activity Type,{stats.MostCommonActivityType}");
            return builder.ToString();
        }

        public async Task<string> GetStatisticsJson(Guid userId) {
            var stats = await GetStatistics(userId);
            return JsonSerializer.Serialize(stats, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private async Task<int> GetUserSessionsCount(Guid userId) {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Sessions
                .Where(s => s.Template.OwnerId == userId)
                .CountAsync();
        }

        private async Task<int> GetUserActivitiesCount(Guid userId) {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Sessions
                .Where(s => s.Template.OwnerId == userId)
                .SelectMany(s => s.Activities)
                .CountAsync();
        }

        private async Task<string?> GetMostCommonActivityType(Guid userId) {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var mostCommonType = await context.Sessions
                .Where(s => s.Template.OwnerId == userId)
                .SelectMany(s => s.Activities)
                .GroupBy(a => a.ActivityType)
                .Select(g => new {
                    ActivityType = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Select(g => g.ActivityType)
                .FirstOrDefaultAsync();

            return mostCommonType;
        }
    }
}