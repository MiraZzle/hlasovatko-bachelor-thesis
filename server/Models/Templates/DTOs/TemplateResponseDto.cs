using server.Models.Activities.DTOs;
using server.Models.Enums;
using server.Utils;
using System.Text.Json.Serialization;

namespace server.Models.Templates.DTOs
{
    public class TemplateResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        [JsonConverter(typeof(SessionModeJsonConverter))]
        public SessionMode SessionPacing { get; set; }
        public bool ResultsVisibleDefault { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ActivityBankResponseDto> Definition { get; set; } = new();
    }
}
