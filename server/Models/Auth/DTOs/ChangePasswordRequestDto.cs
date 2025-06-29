using System.ComponentModel.DataAnnotations;

namespace server.Models.Auth.DTOs
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
