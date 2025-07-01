using server.Models.Auth.server.Models.Auth;
using System.ComponentModel.DataAnnotations;
using server.Models.Activities;

namespace server.Models.Auth
{
    /// <summary>
    /// Entity representing the user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The users email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        /// <summary>
        /// The users full display name - e.g. "Admin Velky".
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// The hashed password for the user.
        /// </summary>
        public string PasswordHash { get; set; } = default!;

        /// <summary>
        /// Navigation property to the users API key (if exists).
        /// </summary>
        public virtual ApiKey? ApiKey { get; set; }

        /// <summary>
        /// Navigation property to the collection of activities owned by the user in their activity bank.
        /// </summary>
        public virtual ICollection<BankActivity> BankActivities { get; set; } = new List<BankActivity>();

    }
}
