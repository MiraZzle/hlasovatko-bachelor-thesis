using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace server.Models.Activities
{
    public class Activity : IActivity {
        [Key]
        public Guid ActivityId { get; set; } = Guid.NewGuid();
        public string ActivityName { get; set; } = string.Empty;
        public virtual string ActivityType => throw new NotImplementedException(); // Ensure derived classes implement it
        // public JsonDocument Definition { get; set; }

        public static Activity Create(JsonElement definition) {
            return ActivityFactory.CreateActivity(definition);
        }
    }
}
