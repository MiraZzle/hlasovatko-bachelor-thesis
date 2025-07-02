using server.Models.Answers;
using System.Text.Json;

namespace server.Services.Analytics
{
    /// <summary>
    /// Interface for processing and aggregating results for a specific activity type.
    /// </summary>
    public interface IActivityResultProcessor
    {
        /// <summary>
        /// The activity type this processor handles.
        /// </summary>
        string ActivityType { get; }

        /// <summary>
        /// Processes and aggregates results for the specified activity definition and its answers.
        /// </summary>
        /// <param name="activityDefinition">
        /// The activity definition as a JSON string. The structure depends on the activity type and typically describes the activity's content and rules.
        /// </param>
        /// <param name="answers">
        /// The list of <see cref="Answer"/> objects submitted for this activity.
        /// </param>
        /// <returns>
        /// A <see cref="JsonElement"/> representing the aggregated results.
        /// The structure of the result depends on the activity type and implementation.
        /// </returns>
        JsonElement Process(string activityDefinition, List<Answer> answers);
    }
}
