using server.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.Activities
{
    /// <summary>
    /// Full activity model representing an activity in the system.
    /// </summary>
    public class Activity : IActivity
    {
        /// <summary>
        /// The unique identifier of the activity.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The title of the activity - should be the question asked.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The type or category of the activity.
        /// </summary>
        [Required]
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// The definition of the activity as a JSON object.
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        public string Definition { get; set; } = "{}";

        /// <summary>
        /// List of tags for categorizing the activity.
        /// </summary>
        public List<string> Tags { get; set; } = new();
    }
}