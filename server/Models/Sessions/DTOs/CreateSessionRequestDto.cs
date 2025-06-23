using server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace server.Models.Sessions.DTOs
{
    public class CreateSessionRequestDto
    {
        [Required]
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Optional: The future date and time when the session should become active.
        /// </summary>
        public DateTime? ActivationDate { get; set; }

        /// <summary>
        /// Optional: The pacing mode for the session. Defaults to TeacherPaced if not provided.
        /// </summary>
        public SessionMode? Mode { get; set; }
    }
}
