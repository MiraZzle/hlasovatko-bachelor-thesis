using server.Models.Activities;
using server.Models.Templates;
using server.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Template
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<Activity> Definition { get; set; } = new();
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public int Version { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public TemplateSettings Settings { get; set; } = new();
    }
}