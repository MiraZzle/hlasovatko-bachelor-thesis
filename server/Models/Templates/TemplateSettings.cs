using server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace server.Models.Templates
{
    public class TemplateSettings
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new();

        public SessionMode SessionPacing { get; set; }

        public bool ResultsVisibleDefault { get; set; }

        public Guid TemplateId { get; set; }
        public virtual Template Template { get; set; } = null!;
    }
}
