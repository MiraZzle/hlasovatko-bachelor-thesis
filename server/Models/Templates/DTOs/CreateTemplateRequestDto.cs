using server.Models.Activities.DTOs;
using System.ComponentModel.DataAnnotations;

namespace server.Models.Templates.DTOs
{
    public class CreateTemplateRequestDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// A list of full activity definitions to be copied into the new template.
        /// </summary>
        [Required]
        public List<ActivityBankRequestDto> Activities { get; set; } = new();
    }
}
