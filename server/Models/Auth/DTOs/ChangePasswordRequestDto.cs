using System.ComponentModel.DataAnnotations;

namespace server.Models.Auth.DTOs
{
    /// <summary>
    /// DTO for requesting a password change.
    /// </summary>
    public class ChangePasswordRequestDto
    {
        /// <summary>
        /// The users current password.
        /// </summary>
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// The new password to set for the user. Must be at least 6 characters long.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
