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

        /// <summary>
        /// The type of the activity, which corresponds to a JSON schema file
        /// </summary>
        [Required]
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// A JSON string containing the specific definition for this activity.
        /// Validated against the schema defined by ActivityType.
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        public string Definition { get; set; } = "{}";
    }
}