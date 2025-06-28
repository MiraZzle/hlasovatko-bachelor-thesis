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
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ActivityType { get; set; } = string.Empty;

        [Required]
        public JsonElement Definition { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
