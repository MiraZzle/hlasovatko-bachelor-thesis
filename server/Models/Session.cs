using server.Models.Activities;
using System.Text.Json;

namespace server.Entities
{
    public class Session {
        public Guid SessionId { get; set; } = new Guid();
        public string sessionName { get; set; }

        private List<Activity> _activities = new List<Activity>();

        public Session() { }

        public void InitializeFromJson(JsonElement definition) {
            sessionName = definition.GetProperty("title").GetString();
            var activities = definition.GetProperty("activities");

            foreach (var activity in activities.EnumerateArray()) {
                _activities.Add(Activity.Create(activity));
            }
        }

        public void Start() {
            // add session logic
        }
    }
}
