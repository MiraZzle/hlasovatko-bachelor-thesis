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
        }

        public void Start() {
            // add session logic
        }
    }
}
