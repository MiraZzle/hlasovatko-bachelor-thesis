using server.Models.Activities;
using server.Models.Activities.DTOs;

namespace server.Services
{
    /// <summary>
    /// Service interface for managing activities.
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// Adds a new activity to the bank for a specific owner.
        /// </summary>
        /// <param name="dto">The activity DTO containing activity details.</param>
        /// <param name="ownerId">The unique id of the activity owner.</param>
        /// <returns>The created <see cref="ActivityResponseDto"/>.</returns>
        Task<ActivityResponseDto> AddToBankAsync(ActivityRequestDto dto, Guid ownerId);

        /// <summary>
        /// Retrieves all activities from the bank for a specific owner.
        /// </summary>
        /// <param name="ownerId">The unique id of the activity owner.</param>
        /// <returns>A collection of <see cref="ActivityResponseDto"/>.</returns>
        Task<IEnumerable<ActivityResponseDto>> GetBankAsync(Guid ownerId);

        /// <summary>
        /// Validates the definition of an activity based on its type.
        /// </summary>
        /// <param name="activityType">The type of the activity.</param>
        /// <param name="definition">The JSON definition of the activity.</param>
        Task ValidateActivityDefinitionAsync(string activityType, string definition);

        /// <summary>
        /// Maps an <see cref="IActivity"/> instance to an <see cref="ActivityResponseDto"/>.
        /// </summary>
        /// <param name="activity">The activity to map.</param>
        /// <returns>The mapped <see cref="ActivityResponseDto"/>.</returns>
        ActivityResponseDto MapActivityToDto(IActivity activity);

        /// <summary>
        /// Validates the answer definition for a given activity type.
        /// </summary>
        /// <param name="activityType">The type of the activity.</param>
        /// <param name="answerJson">The JSON answer definition.</param>
        Task ValidateAnswerDefinitionAsync(string activityType, string answerJson);
    }
}
