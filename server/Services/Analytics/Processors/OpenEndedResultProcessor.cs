using server.Models.Answers;
using System.Text.Json;

namespace server.Services.Analytics.Processors
{
    public class OpenEndedResultProcessor : IActivityResultProcessor
    {
        public string ActivityType => "open_ended";

        public JsonElement Process(string activityDefinition, List<Answer> answers) {
            // Simply extract text strings and put them into enumerable
            var texts = answers.Select(a => JsonDocument.Parse(a.AnswerJson).RootElement.GetProperty("text").GetString()).ToList();
            return JsonSerializer.SerializeToElement(texts);
        }
    }
}
