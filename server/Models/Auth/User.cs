using server.Models.Auth.server.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace server.Models.Auth
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

        // Navigation property for the one-to-one relationship
        // An ApiKey is optional (nullable) for a new user
        public virtual ApiKey? ApiKey { get; set; }
    }
}
