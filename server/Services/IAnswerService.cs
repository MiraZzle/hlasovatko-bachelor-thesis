using server.Models.Answers.DTOs;

namespace server.Services
{
    public interface IAnswerService
    {
        Task<AnswerResponseDto?> CreateAnswerAsync(Guid sessionId, CreateAnswerRequestDto answerDto);
        Task<IEnumerable<AnswerResponseDto>> GetAnswersForActivityAsync(Guid sessionId, Guid activityId, Guid ownerId);
        Task<IEnumerable<AnswerResponseDto>> GetAllAnswersForSessionAsync(Guid sessionId, Guid ownerId);
        Task<IEnumerable<ActivityResultDto>> GetAggregatedResultsForSessionAsync(Guid sessionId, Guid ownerId);
        Task<ActivityResultDto?> GetAggregatedResultsForActivityAsync(Guid sessionId, Guid activityId);
    }
}
