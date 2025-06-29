using System.ComponentModel.DataAnnotations;
using server.Models.Activities.DTOs;

namespace server.Models.Templates.DTOs
{
    public class UpdateTemplateDto
    {
        [Required]
        public UpdateTemplateSettingsDto Settings { get; set; } = new();
        [Required]
        public List<ActivityRequestDto> Definition { get; set; } = new();
    }
}
