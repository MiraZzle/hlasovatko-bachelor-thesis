using server.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using server.Utils;

namespace server.Models.Sessions.DTOs
{
    public class CreateSessionRequestDto
    {
        [Required]
        public Guid TemplateId { get; set; }

        public DateTime? ActivationDate { get; set; }

        [JsonConverter(typeof(SessionModeJsonConverter))]
        public SessionMode? Mode { get; set; }
        public string? Title { get; set; }

    }
}
