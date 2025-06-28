using server.Models.Answers;
using System.Text.Json;
using server.Models.Activities;

namespace server.Services.Analytics
{
    public interface IAnalyticsProcessor
    {
        JsonElement ProcessResults(Activity activity, List<Answer> answers);
    }
}
