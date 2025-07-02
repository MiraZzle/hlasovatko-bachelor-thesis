using server.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using server.Utils;

namespace server.Models.Sessions.DTOs
{
    /// <summary>
    /// DTO for creating a new session based on a template.
    /// </summary>
    public class CreateSessionRequestDto
    {
        /// <summary>
        /// The unique identifier of the template to base the session on.
        /// </summary>
        [Required]
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Optional activation date for the session. If not provided, the session will be created in a planned state.
        /// </summary>
        public DateTime? ActivationDate { get; set; }

        /// <summary>
        /// The pacing mode for the session (TeacherPaced or StudentPaced).
        /// </summary>
        [JsonConverter(typeof(SessionModeJsonConverter))]
        public SessionMode? Mode { get; set; }

        /// <summary>
        /// The optional title for the session.
        /// </summary>
        public string? Title { get; set; }

    }
}
