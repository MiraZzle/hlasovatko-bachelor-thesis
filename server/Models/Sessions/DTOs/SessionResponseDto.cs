using server.Models.Activities.DTOs;
using server.Models.Enums;
using server.Utils;
using System.Text.Json.Serialization;

namespace server.Models.Sessions.DTOs
{
    /// <summary>
    /// DTO for returning the entire session details to the client.
    /// </summary>
    public class SessionResponseDto
    {
        /// <summary>
        /// The unique identifier of the session.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The title of the session.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The join code for participants to join the session.
        /// </summary>
        public string JoinCode { get; set; } = string.Empty;

        /// <summary>
        /// The current status of the session.
        /// </summary>
        [JsonConverter(typeof(SessionStatusJsonConverter))]
        public SessionStatus Status { get; set; }

        /// <summary>
        /// The list of activities included in the session.
        /// </summary>
        public List<ActivityResponseDto> Activities { get; set; } = new();

        /// <summary>
        /// The date and time when the session becomes active, or null if not scheduled.
        /// </summary>
        public DateTime? ActivationDate { get; set; }

        /// <summary>
        /// The pacing mode for the session (TeacherPaced or StudentPaced).
        /// </summary>
        [JsonConverter(typeof(SessionModeJsonConverter))]
        public SessionMode Mode { get; set; }

        /// <summary>
        /// The number of participants currently in the session.
        /// </summary>
        public int Participants { get; set; }

        /// <summary>
        /// The index of the current activity in the session, or null if not started.
        /// </summary>
        public int? CurrentActivity { get; set; }

        /// <summary>
        /// The unique identifier of the template from which this session was created.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// The version of the template used when this session was created.
        /// </summary>
        public int TemplateVersion { get; set; }

        /// <summary>
        /// The date and time when the session was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }
    }
}
