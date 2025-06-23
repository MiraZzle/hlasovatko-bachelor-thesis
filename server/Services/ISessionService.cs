using server.Models.Sessions.DTOs;

namespace server.Services
{
    public interface ISessionService
    {
        Task<SessionResponseDto> CreateSessionFromTemplateAsync(CreateSessionRequestDto request, Guid ownerId);
        Task<SessionResponseDto?> GetSessionByIdAsync(Guid sessionId, Guid ownerId);
        Task<SessionResponseDto?> StartSessionAsync(Guid sessionId, Guid ownerId);
        Task<SessionResponseDto?> StopSessionAsync(Guid sessionId, Guid ownerId);
        Task<SessionResponseDto?> NextActivityAsync(Guid sessionId, Guid ownerId);
    }
}
