using server.Models.Activities;
using server.Models.Templates;
using server.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    /// <summary>
    /// Entity representing a reusable template for creating sessions.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// The unique identifier for the template.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The unique identifier of the user who owns this template.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Navigation property to the owner of the template.
        /// </summary>
        public virtual User Owner { get; set; } = null!;

        /// <summary>
        /// The version number of the template.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The date and time when the template was created.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The settings for the template.
        /// </summary>
        public virtual TemplateSettings Settings { get; set; } = null!;

        /// <summary>
        /// The collection of activities that define the templates structure.
        /// </summary>
        /// <remarks>
        /// Each activity includes its own metadata and definition.
        /// </remarks>
        public virtual ICollection<Activity> Definition { get; set; } = new List<Activity>();

        /// <summary>
        /// Stores the ordered list of activity ids for this template.
        /// </summary>
        public List<Guid> ActivityOrder { get; set; } = new();
    }
}