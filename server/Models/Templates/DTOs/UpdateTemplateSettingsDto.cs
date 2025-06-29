using server.Models.Enums;
using server.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace server.Models.Templates.DTOs
{
    public class UpdateTemplateSettingsDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        [JsonConverter(typeof(SessionModeJsonConverter))]
        public SessionMode SessionPacing { get; set; }
        public bool ResultsVisibleDefault { get; set; }
    }
}
