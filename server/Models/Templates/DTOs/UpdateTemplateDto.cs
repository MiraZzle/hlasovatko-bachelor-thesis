using System.ComponentModel.DataAnnotations;
using server.Models.Activities.DTOs;

namespace server.Models.Templates.DTOs
{
    /// <summary>
    /// DTO for updating an existing template.
    /// </summary>
    public class UpdateTemplateDto
    {
        /// <summary>
        /// The updated settings for the template.
        /// </summary>
        [Required]
        public UpdateTemplateSettingsDto Settings { get; set; } = new();

        /// <summary>
        /// The updated list of activities that define the templates structure.
        /// </summary>
        /// <remarks>
        /// Each activity must include a title, type, definition (as JSON) according to schema, and optional tags.
        /// </remarks>
        [Required]
        public List<ActivityRequestDto> Definition { get; set; } = new();
    }
}
