using server.Models.Activities;
using server.Models.Sessions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.Answers
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; } = null!;

        [Required]
        public Guid ParticipantId { get; set; }

        [Required]
        [Column(TypeName = "jsonb")]
        public string AnswerJson { get; set; } = "{}";

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
