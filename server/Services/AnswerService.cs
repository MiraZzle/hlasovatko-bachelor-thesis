using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models.Activities;
using server.Models.Activities.DTOs;
using server.Models.Answers;
using server.Models.Answers.DTOs;
using server.Models.Enums;
using server.Services.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace server.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly AppDbContext _context;
        private readonly IActivityService _activityService;
        private readonly IAnalyticsProcessor _analyticsProcessor;

        public AnswerService(AppDbContext context, IActivityService activityService, IAnalyticsProcessor analyticsProcessor) {
            _context = context;
            _activityService = activityService;
            _analyticsProcessor = analyticsProcessor;
        }

        private ActivityResponseDto MapActivityToDto(Activity activity) => new() {
            Id = activity.Id,
            Title = activity.Title,
            ActivityType = activity.ActivityType,
            Definition = JsonDocument.Parse(activity.Definition).RootElement,
            Tags = activity.Tags
        };

        public async Task<AnswerResponseDto?> CreateAnswerAsync(Guid sessionId, CreateAnswerRequestDto answerDto) {
            var session = await _context.Sessions
                .Include(s => s.Activities)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            // Check if session exists and is active
            if (session == null || session.Status != SessionStatus.Active) {
                return null;
            }

            var activity = session.Activities.FirstOrDefault(a => a.Id == answerDto.ActivityId);
            if (activity == null) {
                return null;
            }

            var answerJsonString = JsonSerializer.Serialize(answerDto.AnswerJson);
            await _activityService.ValidateAnswerDefinitionAsync(activity.ActivityType, answerJsonString);

            var newAnswer = new Answer {
                ActivityId = answerDto.ActivityId,
                ParticipantId = answerDto.ParticipantId,
                AnswerJson = answerJsonString,
                Timestamp = DateTime.UtcNow
            };

            _context.Answers.Add(newAnswer);
            await _context.SaveChangesAsync();

            session.Participants++;
            await _context.SaveChangesAsync();

            return new AnswerResponseDto {
                Id = newAnswer.Id,
                ActivityId = newAnswer.ActivityId,
                ParticipantId = newAnswer.ParticipantId,
                AnswerJson = JsonSerializer.Deserialize<JsonElement>(newAnswer.AnswerJson),
                Timestamp = newAnswer.Timestamp
            };
        }

        public async Task<IEnumerable<ActivityResultDto>> GetAggregatedResultsForSessionAsync(Guid sessionId, Guid ownerId) {
            var session = await _context.Sessions
                .Include(s => s.Activities)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.Template.OwnerId == ownerId);

            if (session == null)
                return new List<ActivityResultDto>();

            var activityIds = session.Activities.Select(a => a.Id).ToList();
            var allAnswers = await _context.Answers
                .Where(a => activityIds.Contains(a.ActivityId))
                .AsNoTracking()
                .ToListAsync();

            var answersByActivityId = allAnswers.GroupBy(a => a.ActivityId).ToDictionary(g => g.Key, g => g.ToList());

            var results = new List<ActivityResultDto>();
            foreach (var activity in session.Activities) {
                var answersForThisActivity = answersByActivityId.GetValueOrDefault(activity.Id, new List<Answer>());
                results.Add(new ActivityResultDto {
                    ActivityRef = MapActivityToDto(activity),
                    Results = _analyticsProcessor.ProcessResults(activity, answersForThisActivity)
                });
            }

            return results;
        }

        public async Task<ActivityResultDto?> GetAggregatedResultsForActivityAsync(Guid sessionId, Guid activityId) {
            var session = await _context.Sessions
                .Include(s => s.Activities)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
                return null;

            var activity = session.Activities.FirstOrDefault(a => a.Id == activityId);
            if (activity == null)
                return null;

            var answers = await _context.Answers
                .Where(a => a.ActivityId == activityId)
                .AsNoTracking()
                .ToListAsync();

            return new ActivityResultDto {
                ActivityRef = MapActivityToDto(activity),
                Results = _analyticsProcessor.ProcessResults(activity, answers)
            };
        }

        public async Task<IEnumerable<AnswerResponseDto>> GetAnswersForActivityAsync(Guid sessionId, Guid activityId, Guid ownerId) {
            var isAuthorized = await _context.Sessions
                .AnyAsync(s => s.Id == sessionId &&
                               s.Template.OwnerId == ownerId &&
                               s.Activities.Any(a => a.Id == activityId));

            if (!isAuthorized) {
                return new List<AnswerResponseDto>();
            }

            // Get raw data from db
            var rawAnswers = await _context.Answers
                .Where(a => a.ActivityId == activityId)
                .Select(a => new {
                    a.Id,
                    a.ActivityId,
                    a.ParticipantId,
                    a.AnswerJson,
                    a.Timestamp
                })
                .ToListAsync();

            return rawAnswers.Select(a => new AnswerResponseDto {
                Id = a.Id,
                ActivityId = a.ActivityId,
                ParticipantId = a.ParticipantId,
                AnswerJson = JsonDocument.Parse(a.AnswerJson).RootElement,
                Timestamp = a.Timestamp
            });
        }

        public async Task<IEnumerable<AnswerResponseDto>> GetAllAnswersForSessionAsync(Guid sessionId, Guid ownerId) {
            var session = await _context.Sessions
                .Include(s => s.Activities)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.Template.OwnerId == ownerId);

            if (session == null) {
                return new List<AnswerResponseDto>();
            }

            var activityIds = session.Activities.Select(a => a.Id).ToList();

            if (!activityIds.Any()) {
                return new List<AnswerResponseDto>();
            }

            var rawAnswers = await _context.Answers
                .Where(a => activityIds.Contains(a.ActivityId))
                .Select(a => new {
                    a.Id,
                    a.ActivityId,
                    a.ParticipantId,
                    a.AnswerJson,
                    a.Timestamp
                })
                .ToListAsync();

            return rawAnswers.Select(a => new AnswerResponseDto {
                Id = a.Id,
                ActivityId = a.ActivityId,
                ParticipantId = a.ParticipantId,
                AnswerJson = JsonDocument.Parse(a.AnswerJson).RootElement,
                Timestamp = a.Timestamp
            });
        }
    }
}
