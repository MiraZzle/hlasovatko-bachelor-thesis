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
using server.Utils;

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

        public async Task<ActivityResponseDto> AddToBankAsync(ActivityRequestDto dto, Guid ownerId) {
            // Re-serialize the JsonElement to a string for validation and storage.
            var definitionJson = JsonSerializer.Serialize(dto.Definition);
            await ValidateActivityDefinitionAsync(dto.ActivityType, definitionJson);

            var activity = new BankActivity {
                OwnerId = ownerId,
                Title = dto.Title,
                ActivityType = dto.ActivityType,
                Definition = definitionJson, // Save the stringified version
                Tags = dto.Tags
            };

            _context.BankActivities.Add(activity);
            await _context.SaveChangesAsync();

            return MapActivityToDto(activity);
        }

        public async Task ValidateActivityDefinitionAsync(string activityType, string definitionJson) {
            var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", "Activities", $"{activityType}.json");
            await JsonSchemaValidator.ValidateAsync(definitionJson, schemaPath, "activity definition", _logger);
        }

        public async Task ValidateAnswerDefinitionAsync(string activityType, string answerJson) {
            var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", "Answers", $"{activityType}_answer.json");
            await JsonSchemaValidator.ValidateAsync(answerJson, schemaPath, "answer definition", _logger);
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetBankAsync(Guid ownerId) {
            var activitiesFromDb = await _context.BankActivities
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

        public ActivityResponseDto MapActivityToDto(IActivity activity) {
            return new ActivityResponseDto {
                Id = activity.Id,
                Title = activity.Title,
                ActivityType = activity.ActivityType,
                Definition = JsonDocument.Parse(activity.Definition).RootElement,
                Tags = activity.Tags
            };
        }
    }
}
