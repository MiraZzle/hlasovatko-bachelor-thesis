using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace server.Models.Activities.DTOs
{
    /// <summary>
    /// DTO for creating or updating an activity.
    /// The Definition is passed as a string and validated on the backend.
    /// </summary>
    public class ActivityRequestDto
    {
        /// <summary>
        /// The title of the activity.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The type of the activity.
        /// </summary>
        [Required]
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// The definition of the activity as a JSON object.
        /// </summary>
        [Required]
        public JsonElement Definition { get; set; }

        /// <summary>
        /// Optional list of tags for categorizing the activity.
        /// </summary>
        public List<string> Tags { get; set; } = new();
    }
}
