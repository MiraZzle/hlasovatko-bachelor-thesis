using server.Models.Answers;
using System.Text.Json;

namespace server.Services.Analytics.Processors
{
    public class MultipleChoiceResultProcessor : IActivityResultProcessor
    {
        public string ActivityType => "multiple_choice";

        public JsonElement Process(string activityDefinition, List<Answer> answers) {
            var pollDef = JsonDocument.Parse(activityDefinition).RootElement;
            var options = pollDef.GetProperty("options").EnumerateArray()
                                 .ToDictionary(opt => opt.GetProperty("id").GetString()!,
                                               opt => new { id = opt.GetProperty("id").GetString(), text = opt.GetProperty("text").GetString(), count = 0 });

            foreach (var answer in answers) {
                var answerDoc = JsonDocument.Parse(answer.AnswerJson).RootElement;
                if (answerDoc.TryGetProperty("selectedOptionIds", out var idsElement)) {
                    var ids = idsElement.EnumerateArray().Select(e => e.GetString());
                    foreach (var selectedId in ids) {
                        if (selectedId != null && options.ContainsKey(selectedId)) {
                            var current = options[selectedId];
                            options[selectedId] = new { id = current.id, text = current.text, count = current.count + 1 };
                        }
                    }
                }
            }
            return JsonSerializer.SerializeToElement(options.Values);
        }
    }
}
