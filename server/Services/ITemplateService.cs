using server.Models.Templates.DTOs;

namespace server.Services
{
    public interface ITemplateService
    {
        Task<TemplateResponseDto> CreateTemplateAsync(CreateTemplateRequestDto templateDto, Guid ownerId);
        Task<TemplateResponseDto?> GetTemplateByIdAsync(Guid id);
        Task<IEnumerable<TemplateResponseDto>> GetAllTemplatesForUserAsync(Guid ownerId);
        Task<bool> DeleteTemplateAsync(Guid id, Guid ownerId);
    }
}
