using server.Models.Answers;
using System.Text.Json;

namespace server.Services.Analytics.Processors
{
    public class PollResultProcessor : IActivityResultProcessor
    {
        public string ActivityType => "poll";

        public JsonElement Process(string activityDefinition, List<Answer> answers) {
            Console.WriteLine($"Processing poll results for {answers.Count} answers with definition: {activityDefinition}");

            var pollDef = JsonDocument.Parse(activityDefinition).RootElement;

            if (!pollDef.TryGetProperty("options", out var optionsElement)) {
                return JsonSerializer.SerializeToElement(new List<object>());
            }

            var options = optionsElement.EnumerateArray()
                                 .ToDictionary(
                                     opt => opt.GetProperty("id").GetString() ?? "",
                                     opt => new {
                                         id = opt.GetProperty("id").GetString() ?? "",
                                         text = opt.GetProperty("text").GetString() ?? "Option",
                                         count = 0
                                     });

            foreach (var answer in answers) {
                var answerDoc = JsonDocument.Parse(answer.AnswerJson).RootElement;
                if (answerDoc.TryGetProperty("selectedOptionId", out var idElement) && idElement.GetString() is string selectedId && options.ContainsKey(selectedId)) {
                    var current = options[selectedId];
                    options[selectedId] = new { id = current.id, text = current.text, count = current.count + 1 };
                }
            }
            return JsonSerializer.SerializeToElement(options.Values);
        }
    }
}
