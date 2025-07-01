using server.Models.Activities.DTOs;
using System.Text.Json;

namespace server.Models.Answers.DTOs
{
    /// <summary>
    /// DTO for returning the aggregated results of an activity - for displaying overall answers.
    /// </summary>
    public class ActivityResultDto
    {
        /// <summary>
        /// Reference to the activity, including its metadata and definition.
        /// </summary>
        public ActivityResponseDto ActivityRef { get; set; }

        /// <summary>
        /// The aggregated results for the activity, represented as a JSON object.
        /// </summary>
        public JsonElement Results { get; set; }
    }
}
