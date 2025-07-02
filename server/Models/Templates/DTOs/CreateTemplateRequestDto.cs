using server.Models.Activities.DTOs;
using server.Models.Enums;
using server.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace server.Models.Templates.DTOs
{
    /// <summary>
    /// DTO for creating a new template.
    /// </summary>
    public class CreateTemplateRequestDto
    {
        /// <summary>
        /// The title of the template.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Optional list of tags for categorizing the template.
        /// </summary>
        public List<string> Tags { get; set; } = new();
        [JsonConverter(typeof(SessionModeJsonConverter))]

        /// <summary>
        /// The pacing mode for sessions created from this template (either TeacherPaced or StudentPaced).
        /// </summary>
        public SessionMode SessionPacing { get; set; }

        /// <summary>
        /// Indicates whether results are visible to participants by default in sessions created from this template.
        /// </summary>
        public bool ResultsVisibleDefault { get; set; }

        /// <summary>
        /// The list of activities that define the templates structure.
        /// </summary>
        /// <remarks>
        /// Each activity must include a title, type, definition (as JSON) according to schema, and optional tags.
        /// </remarks>
        [Required]
        public List<ActivityRequestDto> Activities { get; set; } = new();
    }
}
