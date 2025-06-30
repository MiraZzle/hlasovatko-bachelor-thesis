using server.Data;
using server.Entities;
using server.Models.Activities.DTOs;
using server.Models.Enums;
using server.Models.Sessions.DTOs;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using server.Models.Activities;

namespace server.Services
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _context;

        public SessionService(AppDbContext context) {
            _context = context;
        }

        public async Task<SessionResponseDto?> GetSessionByIdAsync(Guid sessionId) {
            var session = await _context.Sessions
                .Include(s => s.Template)
                .Include(s => s.Activities)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            return session == null ? null : MapSessionToDto(session);
        }

        public async Task<bool> DeleteSessionAsync(Guid sessionId, Guid ownerId) {
            var session = await _context.Sessions
                .Include(s => s.Template)
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.Template.OwnerId == ownerId);
            if (session == null) {
                return false;
            }
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SessionResponseDto>> GetAllSessionsAsync(Guid ownerId) {
            var sessions = await _context.Sessions
                .Include(s => s.Template)
                .Where(s => s.Template.OwnerId == ownerId)
                .AsNoTracking()
                .ToListAsync();

            return sessions.Select(MapSessionToDto);
        }

        public async Task<IEnumerable<SessionResponseDto>> GetSessionsByTemplateAsync(Guid templateId, Guid ownerId) {
            var sessions = await _context.Sessions
                .Include(s => s.Template)
                .Where(s => s.TemplateId == templateId && s.Template.OwnerId == ownerId)
                .AsNoTracking()
                .ToListAsync();

            return sessions.Select(MapSessionToDto);
        }

        public async Task<SessionResponseDto> CreateSessionFromTemplateAsync(CreateSessionRequestDto request, Guid ownerId) {
            var template = await _context.Templates
                .Include(t => t.Definition)
                .Include(t => t.Settings)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.TemplateId && t.OwnerId == ownerId);

            if (template == null) {
                throw new Exception("Template not found or you do not have permission to access it.");
            }

            // check if session is planned and format the date
            DateTime? setDate = request.ActivationDate.HasValue
                ? DateTime.SpecifyKind(request.ActivationDate.Value, DateTimeKind.Utc)
                : null;

            var session = new Session {
                Title = !string.IsNullOrEmpty(request.Title) ? request.Title : template.Settings.Title,
                TemplateId = template.Id,
                TemplateVersion = template.Version,
                Status = request.ActivationDate.HasValue ? SessionStatus.Planned : SessionStatus.Inactive,
                JoinCode = await GenerateUniqueJoinCode(),
                ActivationDate = setDate,
                CurrentActivity = null
            };

            session.Activities = template.Definition.Select(activity => new Activity {
                Title = activity.Title,
                ActivityType = activity.ActivityType,
                Definition = activity.Definition,
            }).ToList();

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return MapSessionToDto(session);
        }

        public async Task<SessionResponseDto?> StartSessionAsync(Guid sessionId, Guid ownerId) {
            var session = await FindSessionForManagement(sessionId, ownerId);
            if (session == null || session.Status == SessionStatus.Active)
                return null;

            session.Status = SessionStatus.Active;
            session.CurrentActivity = 0;
            await _context.SaveChangesAsync();

            return MapSessionToDto(session);
        }

        public async Task<SessionResponseDto?> StopSessionAsync(Guid sessionId, Guid ownerId) {
            var session = await FindSessionForManagement(sessionId, ownerId);
            if (session == null || session.Status == SessionStatus.Finished)
                return null;

            session.Status = SessionStatus.Finished;
            session.CurrentActivity = null;
            await _context.SaveChangesAsync();

            return MapSessionToDto(session);
        }

        public async Task<SessionResponseDto?> NextActivityAsync(Guid sessionId, Guid ownerId) {
            var session = await FindSessionForManagement(sessionId, ownerId);
            if (session == null || session.Status != SessionStatus.Active) {
                return null;
            }

            var currentIdx = session.CurrentActivity ?? -1;

            if (currentIdx < session.Activities.Count - 1) {
                session.CurrentActivity = currentIdx + 1;
            }
            else {
                session.Status = SessionStatus.Finished;
                session.CurrentActivity = null;
            }

            await _context.SaveChangesAsync();
            return MapSessionToDto(session);
        }

        private async Task<Session?> FindSessionForManagement(Guid sessionId, Guid ownerId) {
            return await _context.Sessions
                .Include(s => s.Template)
                .Include(s => s.Activities)
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.Template.OwnerId == ownerId);
        }

        public async Task<SessionResponseDto?> GetSessionByJoinCodeAsync(string joinCode) {
            var session = await _context.Sessions
               .Include(s => s.Template)
               .Include(s => s.Activities)
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.JoinCode == joinCode);

            return session == null ? null : MapSessionToDto(session);
        }

        public async Task<ParticipantSessionStateDto?> GetParticipantSessionStateAsync(Guid sessionId) {
            return await _context.Sessions
                .Where(s => s.Id == sessionId)
                .Select(s => new ParticipantSessionStateDto {
                    SessionId = s.Id,
                    Status = s.Status,
                    CurrentActivityId = s.Activities
                                         .OrderBy(a => a.Id)
                                         .Skip(s.CurrentActivity ?? -1)
                                         .Select(a => (Guid?)a.Id)
                                         .FirstOrDefault(),
                    ShowResults = s.Template.Settings.ResultsVisibleDefault
                })
                .FirstOrDefaultAsync();
        }

        private async Task<string> GenerateUniqueJoinCode() {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";
            var random = new Random();
            string code;

            do {
                code = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (await _context.Sessions.AnyAsync(s => s.JoinCode == code));

            return code;
        }

        private SessionResponseDto MapSessionToDto(Session session) {
            return new SessionResponseDto {
                Id = session.Id,
                Title = session.Title,
                TemplateId = session.TemplateId,
                TemplateVersion = session.TemplateVersion,
                Status = session.Status,
                JoinCode = session.JoinCode ?? "",
                ActivationDate = session.ActivationDate,
                Mode = session.Mode,
                Participants = session.Participants,
                CurrentActivity = session.CurrentActivity,
                CreatedAt = session.Created,
                Activities = session.Activities?.Select(a => new ActivityResponseDto {
                    Id = a.Id,
                    Title = a.Title,
                    ActivityType = a.ActivityType,
                    Definition = JsonDocument.Parse(a.Definition).RootElement
                }).ToList() ?? new List<ActivityResponseDto>()
            };
        }

        public async Task<IEnumerable<ActivityResponseDto>?> GetSessionActivitiesAsync(Guid sessionId) {
            var session = await _context.Sessions
                .Include(s => s.Template)
                .Include(s => s.Activities)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null) {
                return null;
            }

            return session.Activities.Select(a => new ActivityResponseDto {
                Id = a.Id,
                Title = a.Title,
                ActivityType = a.ActivityType,
                Definition = JsonDocument.Parse(a.Definition).RootElement,
                Tags = a.Tags
            });
        }
    }
}