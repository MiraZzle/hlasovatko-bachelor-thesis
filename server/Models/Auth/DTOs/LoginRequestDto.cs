using System.ComponentModel.DataAnnotations;

namespace server.Models.Auth.DTOs
{
    /// <summary>
    /// DTO for login requests.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Email address of the user attempting to log in.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password of the user attempting to log in.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
