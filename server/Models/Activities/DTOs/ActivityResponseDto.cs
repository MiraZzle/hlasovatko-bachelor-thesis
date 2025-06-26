using System.Text.Json;

namespace server.Models.Activities.DTOs
{
    /// <summary>
    /// DTO for returning a full activity, including its parsed definition.
    /// </summary>
    public class ActivityResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public JsonElement Definition { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
