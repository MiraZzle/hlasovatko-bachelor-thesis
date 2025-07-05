using server.Entities;
using server.Models.Activities.DTOs;
using server.Models.Sessions.DTOs;
using server.Models.Activities;

namespace server.Services
{
    /// <summary>
    /// Service interface for managing sessions.
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Deletes a session for the specified owner.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="ownerId">The unique id of the session owner.</param>
        /// <returns><c>true</c> if the session was deleted successfully; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteSessionAsync(Guid sessionId, Guid ownerId);

        /// <summary>
        /// Retrieves a session by its unique id.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <returns>The <see cref="SessionResponseDto"/> if found; otherwise, <c>null</c>.</returns>
        Task<SessionResponseDto?> GetSessionByIdAsync(Guid sessionId);

        /// <summary>
        /// Retrieves all sessions for a specific owner.
        /// </summary>
        /// <param name="ownerId">The unique id of the session owner.</param>
        /// <returns>A collection of <see cref="SessionResponseDto"/> representing the owners sessions.</returns>
        Task<IEnumerable<SessionResponseDto>> GetAllSessionsAsync(Guid ownerId);

        /// <summary>
        /// Retrieves all sessions created from a specific template for a given owner.
        /// </summary>
        /// <param name="templateId">The unique id of the template.</param>
        /// <param name="ownerId">The unique id of the session owner.</param>
        /// <returns>A collection of <see cref="SessionResponseDto"/> for the template and owner.</returns>
        Task<IEnumerable<SessionResponseDto>> GetSessionsByTemplateAsync(Guid templateId, Guid ownerId);

        /// <summary>
        /// Creates a new session from a template for the specified owner.
        /// </summary>
        /// <param name="request">The request DTO containing session creation details.</param>
        /// <param name="ownerId">The unique id of the session owner.</param>
        /// <returns>The created <see cref="SessionResponseDto"/>.</returns>
        Task<SessionResponseDto> CreateSessionFromTemplateAsync(CreateSessionRequestDto request, Guid ownerId);

        /// <summary>
        /// Starts a session for the specified owner.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="ownerId">The unique id of the session owner.</param>
        /// <returns>The updated <see cref="SessionResponseDto"/> if started; otherwise, <c>null</c>.</returns>
        Task<SessionResponseDto?> StartSessionAsync(Guid sessionId, Guid ownerId);

        /// <summary>
        /// Stops a session for the specified owner.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="ownerId">The unique id of the session owner.</param>
        /// <returns>The updated <see cref="SessionResponseDto"/> if stopped; otherwise, <c>null</c>.</returns>
        Task<SessionResponseDto?> StopSessionAsync(Guid sessionId, Guid ownerId);

        /// <summary>
        /// Advances the session to the next activity for the specified owner.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="ownerId">The unique id of the session owner.</param>
        /// <returns>The updated <see cref="SessionResponseDto"/> if advanced; otherwise, <c>null</c>.</returns>
        Task<SessionResponseDto?> NextActivityAsync(Guid sessionId, Guid ownerId);

        /// <summary>
        /// Retrieves all activities for a given session.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <returns>A collection of <see cref="ActivityResponseDto"/> if found; otherwise, <c>null</c>.</returns>
        Task<IEnumerable<ActivityResponseDto>?> GetSessionActivitiesAsync(Guid sessionId);

        /// <summary>
        /// Retrieves a session by its join code.
        /// </summary>
        /// <param name="joinCode">The join code for the session.</param>
        /// <returns>The <see cref="SessionResponseDto"/> if found; otherwise, <c>null</c>.</returns>
        Task<SessionResponseDto?> GetSessionByJoinCodeAsync(string joinCode);

        /// <summary>
        /// Retrieves the current state of a session for a participant.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <returns>The <see cref="ParticipantSessionStateDto"/> if found; otherwise, <c>null</c>.</returns>
        Task<ParticipantSessionStateDto?> GetParticipantSessionStateAsync(Guid sessionId);

        /// <summary>
        /// Retrieves the ordered list of activities for a session.
        /// </summary>
        /// <param name="session">The session to get ordered activities for.</param>
        /// <returns></returns>
        IEnumerable<Activity> GetOrderedActivities(Session session);
    }
}
