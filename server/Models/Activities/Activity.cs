using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace server.Models.Activities
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "ActivityType")]
    [JsonDerivedType(typeof(QuizActivity), "Quiz")]
    public abstract class Activity : IActivity {
        [Key]
        public Guid ActivityId { get; set; } = Guid.NewGuid();
        public string ActivityName { get; set; } = string.Empty;
        public abstract string ActivityType { get; }

        public static Activity Create(JsonElement definition) {
            return ActivityFactory.CreateActivity(definition);
        }
    }
}
