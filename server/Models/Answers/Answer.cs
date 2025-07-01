using server.Models.Activities;
using server.Models.Sessions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.Answers
{
    /// <summary>
    /// Entity representing an individual answer submission for an activity.
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// The unique identifier for the answer.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The unique identifier of the activity this answer is for.
        /// </summary>
        [Required]
        public Guid ActivityId { get; set; }

        /// <summary>
        /// Navigation property to the related activity.
        /// </summary>
        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; } = null!;

        /// <summary>
        /// The unique identifier of the participant who submitted the answer.
        /// </summary>
        [Required]
        public Guid ParticipantId { get; set; }

        /// <summary>
        /// The answer content as a JSON string.
        /// </summary>
        /// <remarks>
        /// The structure of this JSON depends on the activity type.
        /// </remarks>
        [Required]
        [Column(TypeName = "jsonb")]
        public string AnswerJson { get; set; } = "{}";

        /// <summary>
        /// The timestamp when the answer was submitted.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
