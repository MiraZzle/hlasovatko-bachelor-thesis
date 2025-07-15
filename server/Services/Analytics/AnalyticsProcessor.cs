using server.Models.Activities;
using server.Models.Answers;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System;


namespace server.Services.Analytics
{

    public class AnalyticsProcessor : IAnalyticsProcessor
    {
        private readonly IReadOnlyDictionary<string, IActivityResultProcessor> _processors;

        /// <summary>
        /// Initializes the processor by discovering all registered IActivityResultProcessor implementations.
        /// </summary>
        public AnalyticsProcessor(IEnumerable<IActivityResultProcessor> processors) {
            _processors = processors.ToDictionary(p => p.ActivityType, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Processes results for an activity by finding the appropriate registered processor.
        /// </summary>
        public JsonElement ProcessResults(Activity activity, List<Answer> answers) {

            if (_processors.TryGetValue(activity.ActivityType, out var processor)) {
                try {
                    return processor.Process(activity.Definition, answers);
                }
                catch (Exception ex) {
                    return JsonSerializer.SerializeToElement(new { error = $"Could not process results for activity type '{activity.ActivityType}'.", 
                        details = ex.Message });
                }
            }

            // Fallback for unknown activity types
            return JsonSerializer.SerializeToElement(new List<object>());
        }
    }
}
