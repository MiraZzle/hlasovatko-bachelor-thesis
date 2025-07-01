using server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace server.Models.Templates
{
    /// <summary>
    /// Entity representing the settings for a template.
    /// </summary>
    public class TemplateSettings
    {
        /// <summary>
        /// The unique identifier for the template settings.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The title of the template.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// List of tags for categorizing the template.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// The pacing mode for sessions created from this template (TeacherPaced or StudentPaced).
        /// </summary>
        public SessionMode SessionPacing { get; set; }

        /// <summary>
        /// Indicates whether results are visible to participants by default in sessions created from this template.
        /// </summary>
        public bool ResultsVisibleDefault { get; set; }

        /// <summary>
        /// The unique identifier of the template to which these settings belong.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Navigation property to the template that owns these settings.
        /// </summary>
        public virtual Template Template { get; set; } = null!;
    }
}
