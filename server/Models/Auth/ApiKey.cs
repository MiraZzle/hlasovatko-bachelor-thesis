namespace server.Models.Auth
{
    using server.Models.Auth;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace server.Models.Auth
    {
        
        public class ApiKey
        {
            [Key]
            public Guid Id { get; set; } = Guid.NewGuid();

            /// <summary>
            /// The hashed version of the API key.
            /// </summary>
            [Required]
            public string KeyHash { get; set; } = string.Empty;

            /// <summary>
            /// The last few characters of the raw key to help the user recognize it.
            /// </summary>
            [Required]
            public string PartialKey { get; set; } = string.Empty;

            public bool IsEnabled { get; set; } = true;
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
            public DateTime? LastUsedDate { get; set; }

            // FK for the 1-1 relationship with User
            public Guid UserId { get; set; }

            [ForeignKey("UserId")]
            public virtual User User { get; set; } = null!;
        }
    }
}
