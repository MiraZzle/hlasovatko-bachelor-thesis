using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace server.Models.Activities
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "ActivityType")]
    public abstract class Activity {
        [Key]
        public Guid ActivityId { get; set; } = Guid.NewGuid();
        public string ActivityName { get; set; } = string.Empty;
        public abstract string ActivityType { get; }
    }
}
