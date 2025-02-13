using System.Text.Json;
namespace server.Models.Activities
{
    public abstract class Activity {
        public Guid activityId = Guid.NewGuid();
        public string activityName;
        public abstract string ActivityType { get; } // must be defined in derived activity

        public static Activity Create(JsonElement definition) {
            return ActivityFactory.CreateActivity(definition);
        }
    }
}
