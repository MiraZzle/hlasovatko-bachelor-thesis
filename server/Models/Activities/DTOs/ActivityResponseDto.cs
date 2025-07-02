using System.Text.Json;

namespace server.Models.Activities.DTOs
{
    /// <summary>
    /// DTO for returning a full activity.
    /// </summary>
    public class ActivityResponseDto
    {
        /// <summary>
        /// The unique identifier of the activity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The title of the activity.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The type of the activity.
        /// </summary>
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// The definition of the activity as a JSON object.
        /// </summary>
        public JsonElement Definition { get; set; }

        /// <summary>
        /// List of tags for categorizing the activity.
        /// </summary>
        public List<string> Tags { get; set; } = new();
    }
}
