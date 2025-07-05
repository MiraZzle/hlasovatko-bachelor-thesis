using server.Models.Answers.DTOs;

namespace server.Services
{
    /// <summary>
    /// Service interface for managing answers to activities.
    /// </summary>
    public interface IAnswerService
    {
        /// <summary>
        /// Creates a new answer for a specific activity within a session.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="answerDto">The DTO containing the answer details.</param>
        /// <param name="participantId">The id of the participant submitting the answer.</param>
        /// <returns>
        /// The created <see cref="AnswerResponseDto"/>, or null if creation failed.
        /// </returns>
        Task<AnswerResponseDto?> CreateAnswerAsync(Guid sessionId, Guid participantId, CreateAnswerRequestDto answerDto);

        /// <summary>
        /// Retrieves all answers for a specific activity within a session and owner context.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="activityId">The unique id of the activity.</param>
        /// <param name="ownerId">The unique id of the owner.</param>
        /// <returns>
        /// A collection of <see cref="AnswerResponseDto"/> representing the answers.
        /// </returns>
        Task<IEnumerable<AnswerResponseDto>> GetAnswersForActivityAsync(Guid sessionId, Guid activityId, Guid ownerId);

        /// <summary>
        /// Retrieves all answers for all activities within a session and owner context.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="ownerId">The unique id of the owner.</param>
        /// <returns>
        /// A collection of <see cref="AnswerResponseDto"/> representing all answers in the session.
        /// </returns>
        Task<IEnumerable<AnswerResponseDto>> GetAllAnswersForSessionAsync(Guid sessionId, Guid ownerId);

        /// <summary>
        /// Retrieves aggregated results for all activities within a session and owner context.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="ownerId">The unique id of the owner.</param>
        /// <returns>
        /// A collection of <see cref="ActivityResultDto"/> representing aggregated results for each activity.
        /// </returns>
        Task<IEnumerable<ActivityResultDto>> GetAggregatedResultsForSessionAsync(Guid sessionId, Guid ownerId);

        /// <summary>
        /// Retrieves aggregated results for a specific activity within a session.
        /// </summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="activityId">The unique id of the activity.</param>
        /// <returns>
        /// The <see cref="ActivityResultDto"/> representing the aggregated results, or <c>null</c> if not found.
        /// </returns>
        Task<ActivityResultDto?> GetAggregatedResultsForActivityAsync(Guid sessionId, Guid activityId);
    }
}
