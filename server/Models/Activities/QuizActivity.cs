using server.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace server.Models.Activities
{
    public class QuizActivity : Activity {
        public override string ActivityType => "Quiz";
        public List<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    }
}
