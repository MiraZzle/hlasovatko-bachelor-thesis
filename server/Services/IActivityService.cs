using server.Models.Activities.DTOs;

namespace server.Services
{
    public interface IActivityService
    {
        Task<ActivityResponseDto> CreateActivityAsync(ActivityRequestDto activityDto);
        Task<ActivityResponseDto?> GetActivityByIdAsync(Guid id);
        Task<IEnumerable<ActivityResponseDto>> GetAllActivitiesAsync();
        Task<ActivityResponseDto?> UpdateActivityAsync(Guid id, ActivityRequestDto activityDto);
        Task<bool> DeleteActivityAsync(Guid id);
    }
}
