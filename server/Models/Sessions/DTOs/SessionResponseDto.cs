using server.Models.Activities.DTOs;
using server.Models.Enums;
using server.Utils;
using System.Text.Json.Serialization;

namespace server.Models.Sessions.DTOs
{
    public class SessionResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string JoinCode { get; set; } = string.Empty;
        [JsonConverter(typeof(SessionStatusJsonConverter))]
        public SessionStatus Status { get; set; }
        public List<ActivityResponseDto> Activities { get; set; } = new();
        public DateTime? ActivationDate { get; set; }

        [JsonConverter(typeof(SessionModeJsonConverter))]
        public SessionMode Mode { get; set; }
        public int Participants { get; set; }
        public int? CurrentActivity { get; set; }
        public Guid TemplateId { get; set; }
        public int TemplateVersion { get; set; }
    }
}
