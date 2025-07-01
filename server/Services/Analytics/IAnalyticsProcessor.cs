using server.Models.Answers;
using System.Text.Json;
using server.Models.Activities;

namespace server.Services.Analytics
{
    /// <summary>
    /// Interface for processing and aggregating results for any give activity and its answers.
    /// </summary>
    public interface IAnalyticsProcessor
    {
        /// <summary>
        /// Processes and aggregates results for the specified activity and its answers.
        /// </summary>
        JsonElement ProcessResults(Activity activity, List<Answer> answers);
    }
}
