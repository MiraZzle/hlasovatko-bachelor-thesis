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
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; } = null!;
        public int Version { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public virtual TemplateSettings Settings { get; set; } = null!;
        public virtual ICollection<Activity> Definition { get; set; } = new List<Activity>();
    }
}