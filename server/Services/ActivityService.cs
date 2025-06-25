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

        public async Task<ActivityBankResponseDto> AddToBankAsync(ActivityBankRequestDto dto, Guid ownerId) {
            await ValidateActivityDefinition(dto.ActivityType, dto.Definition);

            var activity = new Activity {
                OwnerId = ownerId,
                Title = dto.Title,
                ActivityType = dto.ActivityType,
                Definition = dto.Definition,
                Tags = dto.Tags
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return MapActivityToDto(activity);
        }

        public async Task ValidateActivityDefinitionAsync(string activityType, string definitionJson) {
            var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", $"{activityType}.json");
            if (!File.Exists(schemaPath)) {
                throw new Exception($"Schema for activity type '{activityType}' not found.");
            }
            var schema = await JsonSchema.FromJsonAsync(await File.ReadAllTextAsync(schemaPath));
            var errors = schema.Validate(definitionJson);
            if (errors.Any()) {
                throw new Exception($"Invalid activity definition: {string.Join(", ", errors.Select(e => e.Path + ": " + e.Kind))}");
            }
        }

        public async Task<IEnumerable<ActivityBankResponseDto>> GetBankAsync(Guid ownerId) {
            var activitiesFromDb = await _context.Activities
                .Where(a => a.OwnerId == ownerId)
                .AsNoTracking()
                .ToListAsync();

            return activitiesFromDb.Select(a => MapActivityToDto(a));
        }

        private async Task ValidateActivityDefinition(string activityType, string definitionJson) {
            var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", $"{activityType}.json");
            if (!File.Exists(schemaPath)) {
                throw new Exception($"Schema for activity type '{activityType}' not found.");
            }
            var schema = await JsonSchema.FromJsonAsync(await File.ReadAllTextAsync(schemaPath));
            var errors = schema.Validate(definitionJson);
            if (errors.Any()) {
                throw new Exception($"Invalid activity definition: {string.Join(", ", errors.Select(e => e.Path + ": " + e.Kind))}");
            }
        }

        private ActivityBankResponseDto MapActivityToDto(Activity activity) {
            return new ActivityBankResponseDto {
                Id = activity.Id,
                Title = activity.Title,
                ActivityType = activity.ActivityType,
                Definition = JsonDocument.Parse(activity.Definition).RootElement,
                Tags = activity.Tags
            };
        }
    }
}
