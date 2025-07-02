using server.Data;
using server.Models;
using server.Models.Activities.DTOs;
using server.Models.Templates;
using server.Models.Templates.DTOs;
using System.Text.Json;
using server.Models.Activities;
using Microsoft.EntityFrameworkCore;

namespace server.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly AppDbContext _context;
        private readonly IActivityService _activityService;

        public TemplateService(AppDbContext context, IActivityService activityService) {
            _context = context;
            _activityService = activityService;
        }

        public async Task<TemplateResponseDto?> UpdateTemplateAsync(Guid id, UpdateTemplateDto dto, Guid ownerId) {
            var template = await _context.Templates
                .Include(t => t.Definition)
                .FirstOrDefaultAsync(t => t.Id == id && t.OwnerId == ownerId);

            if (template == null) {
                return null;
            }

            _context.Activities.RemoveRange(template.Definition);


            await _context.SaveChangesAsync();


            var templateForUpdate = await _context.Templates
                                            .Include(t => t.Settings)
                                            .FirstOrDefaultAsync(t => t.Id == id);

            if (templateForUpdate == null) {
                return null;
            }

            templateForUpdate.Settings.Title = dto.Settings.Title;
            templateForUpdate.Settings.Tags = dto.Settings.Tags;
            templateForUpdate.Settings.SessionPacing = dto.Settings.SessionPacing;
            templateForUpdate.Settings.ResultsVisibleDefault = dto.Settings.ResultsVisibleDefault;


            var newActivities = new List<Activity>();
            foreach (var req in dto.Definition) {
                await _activityService.ValidateActivityDefinitionAsync(req.ActivityType, req.Definition.ToString());
                newActivities.Add(new Activity {
                    Title = req.Title,
                    ActivityType = req.ActivityType,
                    Definition = req.Definition.ToString(),
                    Tags = req.Tags,
                });
            }

            templateForUpdate.Definition = newActivities;
            _context.Activities.AddRange(newActivities);
            
            await _context.SaveChangesAsync();
            
            return await GetTemplateByIdAsync(id);
        }

        public async Task<TemplateResponseDto> CreateTemplateAsync(CreateTemplateRequestDto templateDto, Guid ownerId) {
            var activitiesForTemplate = new List<Activity>();
            foreach (var activityRequest in templateDto.Activities) {
                await _activityService.ValidateActivityDefinitionAsync(activityRequest.ActivityType, activityRequest.Definition.ToString());
                activitiesForTemplate.Add(new Activity {
                    Title = activityRequest.Title,
                    ActivityType = activityRequest.ActivityType,
                    Definition = activityRequest.Definition.ToString(),
                    Tags = activityRequest.Tags,
                });
            }

            var template = new Template {
                OwnerId = ownerId,
                Settings = new TemplateSettings {
                    Title = templateDto.Title,
                    Tags = templateDto.Tags,
                    SessionPacing = templateDto.SessionPacing,
                    ResultsVisibleDefault = templateDto.ResultsVisibleDefault
                },
                Definition = activitiesForTemplate
            };

            _context.Templates.Add(template);
            await _context.SaveChangesAsync();

            return MapTemplateToDto(template);
        }

        public async Task<TemplateResponseDto?> UpdateTemplateSettingsAsync(Guid id, UpdateTemplateSettingsDto settingsDto, Guid ownerId) {
            var template = await _context.Templates
                .Include(t => t.Settings)
                .FirstOrDefaultAsync(t => t.Id == id && t.OwnerId == ownerId);

            if (template?.Settings == null)
                return null;

            template.Settings.Title = settingsDto.Title;
            template.Settings.Tags = settingsDto.Tags;
            template.Settings.SessionPacing = settingsDto.SessionPacing;
            template.Settings.ResultsVisibleDefault = settingsDto.ResultsVisibleDefault;

            await _context.SaveChangesAsync();

            var updatedTemplate = await GetTemplateByIdAsync(id);
            return updatedTemplate;
        }

        public async Task<TemplateResponseDto?> GetTemplateByIdAsync(Guid id) {
            var template = await _context.Templates
               .Include(t => t.Settings)
               .Include(t => t.Definition)
               .AsNoTracking()
               .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                return null;

            return MapTemplateToDto(template);
        }

        public async Task<IEnumerable<TemplateResponseDto>> GetAllTemplatesForUserAsync(Guid ownerId) {
            var templates = await _context.Templates
                .Include(t => t.Settings)
                .Where(t => t.OwnerId == ownerId)
                .AsNoTracking()
                .ToListAsync();

            return templates.Select(MapTemplateToDto);
        }

        public async Task<bool> DeleteTemplateAsync(Guid id, Guid ownerId) {
            var template = await _context.Templates
                .Include(t => t.Definition)
                .FirstOrDefaultAsync(t => t.Id == id && t.OwnerId == ownerId);

            if (template == null)
                return false;

            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
            return true;
        }

        private TemplateResponseDto MapTemplateToDto(Template template) {
            var response = new TemplateResponseDto {
                Id = template.Id,
                DateCreated = template.DateCreated,
                Definition = template.Definition?.Select(a => new ActivityResponseDto {
                    Id = a.Id,
                    Title = a.Title,
                    ActivityType = a.ActivityType,
                    Definition = JsonDocument.Parse(a.Definition).RootElement,
                    Tags = a.Tags
                }).ToList() ?? new List<ActivityResponseDto>()
            };

            if (template.Settings != null) {
                response.Title = template.Settings.Title;
                response.Tags = template.Settings.Tags;
                response.SessionPacing = template.Settings.SessionPacing;
                response.ResultsVisibleDefault = template.Settings.ResultsVisibleDefault;
            }

            return response;
        }
    }
}
