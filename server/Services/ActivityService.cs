using Microsoft.EntityFrameworkCore;
using NJsonSchema;
using server.Data;
using server.Models.Activities;
using server.Models.Activities.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Services
{
    public class ActivityService : IActivityService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ActivityService> _logger;

        public ActivityService(AppDbContext context, ILogger<ActivityService> logger) {
            _context = context;
            _logger = logger;
        }

        private async Task ValidateActivityDefinition(string activityType, string definitionJson) {
            var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", $"{activityType}.json");

            if (!File.Exists(schemaPath)) {
                throw new Exception($"Schema for activity type '{activityType}' not found.");
            }

            var schemaJson = await File.ReadAllTextAsync(schemaPath);
            var schema = await JsonSchema.FromJsonAsync(schemaJson);

            var errors = schema.Validate(definitionJson);
            if (errors.Any()) {
                var errorMessages = string.Join(", ", errors.Select(e => e.Path + ": " + e.Kind));
                throw new Exception($"Invalid activity definition: {errorMessages}");
            }
        }

        public async Task<ActivityResponseDto> CreateActivityAsync(ActivityRequestDto activityDto) {
            await ValidateActivityDefinition(activityDto.ActivityType, activityDto.Definition);

            var activity = new Activity {
                Title = activityDto.Title,
                ActivityType = activityDto.ActivityType,
                Definition = activityDto.Definition
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return new ActivityResponseDto {
                Id = activity.Id,
                Title = activity.Title,
                ActivityType = activity.ActivityType,
                Definition = JsonDocument.Parse(activity.Definition).RootElement
            };
        }

        public async Task<ActivityResponseDto?> GetActivityByIdAsync(Guid id) {
            var activity = await _context.Activities.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
                return null;

            return new ActivityResponseDto {
                Id = activity.Id,
                Title = activity.Title,
                ActivityType = activity.ActivityType,
                Definition = JsonDocument.Parse(activity.Definition).RootElement
            };
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetAllActivitiesAsync() {
            var activities = await _context.Activities.AsNoTracking().ToListAsync();

            return activities.Select(activity => new ActivityResponseDto {
                Id = activity.Id,
                Title = activity.Title,
                ActivityType = activity.ActivityType,
                Definition = JsonDocument.Parse(activity.Definition).RootElement
            });
        }

        public async Task<ActivityResponseDto?> UpdateActivityAsync(Guid id, ActivityRequestDto activityDto) {
            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.Id == id);
            if (activity == null)
                return null;

            await ValidateActivityDefinition(activityDto.ActivityType, activityDto.Definition);

            activity.Title = activityDto.Title;
            activity.ActivityType = activityDto.ActivityType;
            activity.Definition = activityDto.Definition;

            await _context.SaveChangesAsync();

            return new ActivityResponseDto {
                Id = activity.Id,
                Title = activity.Title,
                ActivityType = activity.ActivityType,
                Definition = JsonDocument.Parse(activity.Definition).RootElement
            };
        }

        public async Task<bool> DeleteActivityAsync(Guid id) {
            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.Id == id);
            if (activity == null)
                return false;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
