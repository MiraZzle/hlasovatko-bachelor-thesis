using server.Models.Activities.DTOs;
using server.Models.Enums;
using server.Utils;
using System.Text.Json.Serialization;

namespace server.Models.Templates.DTOs
{
    /// <summary>
    /// DTO for returning template details to the client.
    /// </summary>
    public class TemplateResponseDto
    {
        /// <summary>
        /// The unique identifier of the template.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The title of the template.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// List of tags for categorizing the template.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// The pacing mode for sessions created from this template.
        /// </summary>
        [JsonConverter(typeof(SessionModeJsonConverter))]
        public SessionMode SessionPacing { get; set; }

        /// <summary>
        /// Indicates whether results are visible to participants by default in sessions created from this template.
        /// </summary>
        public bool ResultsVisibleDefault { get; set; }

        /// <summary>
        /// The date and time when the template was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The list of activities that define the templates structure.
        /// </summary>
        public List<ActivityResponseDto> Definition { get; set; } = new();
    }
}
