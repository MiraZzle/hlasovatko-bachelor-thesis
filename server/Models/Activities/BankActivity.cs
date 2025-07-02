using server.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.Activities
{
    /// <summary>
    /// Represents an activity stored in the users activity bank.
    /// </summary>
    public class BankActivity : IActivity
    {

        /// <summary>
        /// The unique identifier for the activity.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        /// <summary>
        /// The title of the activity.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;


        /// <summary>
        /// The type of the activity.
        /// </summary>
        [Required]
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// The definition of the activity as a JSON string.
        /// </summary>
        /// <remarks>
        /// This property stores the activity's structure, such as options and settings.
        /// The schema depends on the <c>ActivityType</c> and is validated on the backend.
        /// </remarks>
        [Required]
        [Column(TypeName = "jsonb")]
        public string Definition { get; set; } = "{}";

        /// <summary>
        /// Optional list of tags for categorizing the activity.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// The unique identifier of the user who owns this activity.
        /// </summary>
        [Required]
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Navigation property to the owner of the activity.
        /// </summary>
        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; } = null!;
    }
}
