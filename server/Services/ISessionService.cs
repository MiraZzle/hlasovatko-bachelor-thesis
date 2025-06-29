using server.Models.Activities.DTOs;
using server.Models.Sessions.DTOs;

namespace server.Services
{
    public interface ISessionService
    {
        Task<bool> DeleteSessionAsync(Guid sessionId, Guid ownerId);
        Task<SessionResponseDto?> GetSessionByIdAsync(Guid sessionId);
        Task<IEnumerable<SessionResponseDto>> GetAllSessionsAsync(Guid ownerId);
        Task<IEnumerable<SessionResponseDto>> GetSessionsByTemplateAsync(Guid templateId, Guid ownerId);
        Task<SessionResponseDto> CreateSessionFromTemplateAsync(CreateSessionRequestDto request, Guid ownerId);
        Task<SessionResponseDto?> StartSessionAsync(Guid sessionId, Guid ownerId);
        Task<SessionResponseDto?> StopSessionAsync(Guid sessionId, Guid ownerId);
        Task<SessionResponseDto?> NextActivityAsync(Guid sessionId, Guid ownerId);
        Task<IEnumerable<ActivityResponseDto>?> GetSessionActivitiesAsync(Guid sessionId);
        Task<SessionResponseDto?> GetSessionByJoinCodeAsync(string joinCode);
        Task<ParticipantSessionStateDto?> GetParticipantSessionStateAsync(Guid sessionId);
    }
}
