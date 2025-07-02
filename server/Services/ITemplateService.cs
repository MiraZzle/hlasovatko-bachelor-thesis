using server.Models.Templates.DTOs;

namespace server.Services
{
    /// <summary>
    /// Service interface for managing templates.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Creates a new template for the specified owner.
        /// </summary>
        /// <param name="templateDto">The DTO containing template creation details.</param>
        /// <param name="ownerId">The unique id of the template owner.</param>
        /// <returns>The created <see cref="TemplateResponseDto"/>.</returns>
        Task<TemplateResponseDto> CreateTemplateAsync(CreateTemplateRequestDto templateDto, Guid ownerId);

        /// <summary>
        /// Retrieves a template by its unique id.
        /// </summary>
        /// <param name="id">The unique id of the template.</param>
        /// <returns>
        /// The <see cref="TemplateResponseDto"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<TemplateResponseDto?> GetTemplateByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all templates owned by a specific user.
        /// </summary>
        /// <param name="ownerId">The unique id of the template owner.</param>
        /// <returns>
        /// A collection of <see cref="TemplateResponseDto"/> representing the users templates.
        /// </returns>
        Task<IEnumerable<TemplateResponseDto>> GetAllTemplatesForUserAsync(Guid ownerId);

        /// <summary>
        /// Deletes a template by its unique id and owner.
        /// </summary>
        /// <param name="id">The unique id of the template.</param>
        /// <param name="ownerId">The unique id of the template owner.</param>
        /// <returns>
        /// <c>true</c> if the template was deleted successfully; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> DeleteTemplateAsync(Guid id, Guid ownerId);

        /// <summary>
        /// Updates an existing templates definition and settings.
        /// </summary>
        /// <param name="id">The unique id of the template.</param>
        /// <param name="dto">The DTO containing updated template data.</param>
        /// <param name="ownerId">The unique id of the template owner.</param>
        /// <returns>
        /// The updated <see cref="TemplateResponseDto"/> if successful; otherwise, <c>null</c>.
        /// </returns>
        Task<TemplateResponseDto?> UpdateTemplateAsync(Guid id, UpdateTemplateDto dto, Guid ownerId);

        /// <summary>
        /// Updates only the settings of an existing template.
        /// </summary>
        /// <param name="id">The unique id of the template.</param>
        /// <param name="settingsDto">The DTO containing updated template settings.</param>
        /// <param name="ownerId">The unique id of the template owner.</param>
        /// <returns>
        /// The updated <see cref="TemplateResponseDto"/> if successful; otherwise, <c>null</c>.
        /// </returns>
        Task<TemplateResponseDto?> UpdateTemplateSettingsAsync(Guid id, UpdateTemplateSettingsDto settingsDto, Guid ownerId);
    }
}
