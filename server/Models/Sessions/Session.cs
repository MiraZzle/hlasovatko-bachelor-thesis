using server.Models.Activities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace server.Entities
{
    public class Session {
        [Key]
        public Guid SessionId { get; set; } = new Guid();
        public string SessionName { get; set; }

        public List<Activity> Activities { get; set; } = new List<Activity>();

        public Session() { }

        public void InitializeFromJson(JsonElement definition) {
            SessionName = definition.GetProperty("title").GetString();
            var definedActivities = definition.GetProperty("activities");

            foreach (var activity in definedActivities.EnumerateArray()) {
                Activities.Add(Activity.Create(activity));
                Console.WriteLine(activity.GetProperty("type").GetString());
            }
        }

        public void Start() {
            // add session logic
        }
    }
}
