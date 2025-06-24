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

        public async Task<TemplateResponseDto> CreateTemplateAsync(CreateTemplateRequestDto templateDto, Guid ownerId) {
            // placeholder for validation logic

            return null;
            
        }

        public async Task<TemplateResponseDto?> GetTemplateByIdAsync(Guid id) {
            var template = await _context.Templates
               .Include(t => t.Settings)
               .Include(t => t.Definition)
               .AsNoTracking()
               .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                return null;

            return new TemplateResponseDto {
                Id = template.Id,
                Title = template.Settings.Title,
                Tags = template.Settings.Tags,
                Definition = template.Definition.Select(a => new ActivityBankResponseDto {
                    Id = a.Id,
                    Title = a.Title,
                    ActivityType = a.ActivityType,
                    Definition = JsonDocument.Parse(a.Definition).RootElement
                }).ToList()
            };
        }

        public async Task<IEnumerable<TemplateResponseDto>> GetAllTemplatesForUserAsync(Guid ownerId) {
            var templates = await _context.Templates
                .Include(t => t.Settings)
                .Where(t => t.OwnerId == ownerId)
                .AsNoTracking()
                .ToListAsync();

            return templates.Select(template => new TemplateResponseDto {
                Id = template.Id,
                Title = template.Settings.Title,
                Tags = template.Settings.Tags
            });
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
    }
}
