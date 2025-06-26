using server.Models.Activities;
using server.Models.Activities.DTOs;

namespace server.Services
{
    public interface IActivityService
    {
        Task<ActivityResponseDto> AddToBankAsync(ActivityRequestDto dto, Guid ownerId);
        Task<IEnumerable<ActivityResponseDto>> GetBankAsync(Guid ownerId);
        Task ValidateActivityDefinitionAsync(string activityType, string definition);
        ActivityResponseDto MapActivityToDto(IActivity activity);
    }
}
