using server.Models.Answers;
using System.Text.Json;

namespace server.Services.Analytics.Processors
{
    public class ScaleRatingResultProcessor : IActivityResultProcessor
    {
        public string ActivityType => "scale_rating";

        public JsonElement Process(string activityDefinition, List<Answer> answers) {
            var scaleDef = JsonDocument.Parse(activityDefinition).RootElement;
            var min = scaleDef.GetProperty("min").GetInt32();
            var max = scaleDef.GetProperty("max").GetInt32();
            var scaleCounts = Enumerable.Range(min, max - min + 1).ToDictionary(i => i, _ => 0);

            foreach (var answer in answers) {
                var rating = JsonDocument.Parse(answer.AnswerJson).RootElement.GetProperty("value").GetInt32();
                if (scaleCounts.ContainsKey(rating)) {
                    scaleCounts[rating]++;
                }
            }
            return JsonSerializer.SerializeToElement(scaleCounts.Select(kvp => new { rating = kvp.Key, count = kvp.Value }));
        }
    }
}
