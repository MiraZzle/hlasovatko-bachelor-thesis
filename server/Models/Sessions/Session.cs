using server.Models.Activities;
using server.Models.Enums;
using server.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace server.Entities
{
    /// <summary>
    /// Entity representing a session, which is an instance of a template with its own activities and state.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// The unique identifier for the session.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The title of the session.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier of the template from which this session was created.
        /// </summary>
        public Guid TemplateId { get; set; }

        [ForeignKey("TemplateId")]
        public Models.Template Template { get; set; } = null!;

        /// <summary>
        /// The current status of the session (e.g., Planned, Active, Finished).
        /// </summary>
        [Required]
        [JsonConverter(typeof(SessionStatusJsonConverter))]
        public SessionStatus Status { get; set; }

        /// <summary>
        /// The date and time when the session was created.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The join code used by participants to join the session, if applicable.
        /// </summary>
        public string? JoinCode { get; set; }

        /// <summary>
        /// The list of activities included in this session.
        /// </summary>
        public List<Activity> Activities { get; set; } = new();

        /// <summary>
        /// The date and time when the session becomes active, or null if not scheduled.
        /// </summary>
        public DateTime? ActivationDate { get; set; }

        /// <summary>
        /// The pacing mode for the session (TeacherPaced or StudentPaced).
        /// </summary>
        [Required]
        public SessionMode Mode { get; set; }

        /// <summary>
        /// The number of participants currently in the session.
        /// </summary>
        public int Participants { get; set; } = 0;

        /// <summary>
        /// The index of the current activity in the session, or null if not started.
        /// </summary>
        public int? CurrentActivity { get; set; }

        /// <summary>
        /// The version of the template used when this session was created - not used as of now.
        /// </summary>
        public int TemplateVersion { get; set; } = 0;

        /// <summary>
        /// Stores the ordered list of activity ids for this sessions.
        /// </summary>
        public List<Guid> ActivityOrder { get; set; } = new();
    }
}