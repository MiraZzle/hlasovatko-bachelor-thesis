using server.Models.Activities;
using server.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entities
{
    public class Session
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        public Guid TemplateId { get; set; }

        [ForeignKey("TemplateId")]
        public Models.Template Template { get; set; } = null!;

        [Required]
        public SessionStatus Status { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string? JoinCode { get; set; }

        public List<Activity> Activities { get; set; } = new();

        public DateTime? ActivationDate { get; set; }

        [Required]
        public SessionMode Mode { get; set; }

        public int Participants { get; set; } = 0;

        public int? CurrentActivity { get; set; }
    }
}