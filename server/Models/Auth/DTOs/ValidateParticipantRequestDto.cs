using System.ComponentModel.DataAnnotations;

namespace server.Models.Auth.DTOs
{
    /// <summary>
    /// DTO for validating a participants join code.
    /// </summary>
    public class ValidateParticipantRequestDto
    {
        /// <summary>
        /// The unique id of the session the participant is attempting to join.
        /// </summary>
        [Required]
        public Guid SessionId { get; set; }

        /// <summary>
        /// Join code for the participant to validate.
        /// </summary>
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string JoinCode { get; set; } = string.Empty;
    }
}
