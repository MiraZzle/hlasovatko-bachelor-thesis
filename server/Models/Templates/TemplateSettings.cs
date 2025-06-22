using server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace server.Models.Templates
{
    public class TemplateSettings
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public SessionMode SessionPacing { get; set; }
        public bool ResultsVisibleDefault { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
