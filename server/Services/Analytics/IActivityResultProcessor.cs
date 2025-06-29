using server.Models.Answers;
using System.Text.Json;

namespace server.Services.Analytics
{
    public interface IActivityResultProcessor
    {
        string ActivityType { get; }
        JsonElement Process(string activityDefinition, List<Answer> answers);
    }
}
