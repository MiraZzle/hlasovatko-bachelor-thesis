using server.Models.Enums;
using server.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace server.Models.Templates.DTOs
{
    /// <summary>
    /// DTO for updating the settings of a template.
    /// </summary>
    public class UpdateTemplateSettingsDto
    {
        /// <summary>
        /// The updated title of the template.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The updated list of tags for categorizing the template.
        /// </summary>
        public List<string> Tags { get; set; } = new();
        [JsonConverter(typeof(SessionModeJsonConverter))]

        /// <summary>
        /// The pacing mode for sessions created from this template.
        /// </summary>
        public SessionMode SessionPacing { get; set; }

        /// <summary>
        /// Indicates whether results are visible to participants.
        /// </summary>
        public bool ResultsVisibleDefault { get; set; }
    }
}
