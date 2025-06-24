using server.Models.Auth.server.Models.Auth;
using System.ComponentModel.DataAnnotations;
using server.Models.Activities;

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

        public virtual ApiKey? ApiKey { get; set; }
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    }
}
