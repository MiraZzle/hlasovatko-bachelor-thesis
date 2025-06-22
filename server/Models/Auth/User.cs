using System.ComponentModel.DataAnnotations;

namespace server.Models.Users
{
    public class User
    {   
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Name { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;
    }
}
