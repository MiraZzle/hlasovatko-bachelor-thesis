using server.Models.Activities.DTOs;

namespace server.Services
{
    public interface IActivityService
    {
        Task<ActivityBankResponseDto> AddToBankAsync(ActivityBankRequestDto dto, Guid ownerId);
        Task<IEnumerable<ActivityBankResponseDto>> GetBankAsync(Guid ownerId);
        Task ValidateActivityDefinitionAsync(string activityType, string definition);
    }
}
