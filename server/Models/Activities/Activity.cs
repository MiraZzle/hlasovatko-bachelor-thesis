using server.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.Activities
{
    public class Activity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ActivityType { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "jsonb")]
        public string Definition { get; set; } = "{}";

        [Required]
        public Guid OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; } = null!;
        public List<string> Tags { get; set; } = new();
    }
}