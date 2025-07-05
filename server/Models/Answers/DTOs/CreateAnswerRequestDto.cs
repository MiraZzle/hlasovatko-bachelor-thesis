using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace server.Models.Answers.DTOs
{
    /// <summary>
    /// DTO for creating a new answer to an activity.
    /// </summary>
    public class CreateAnswerRequestDto
    {
        /// <summary>
        /// The unique identifier of the activity being answered.
        /// </summary>
        [Required]
        public Guid ActivityId { get; set; }

        /// <summary>
        /// The answer content as a JSON object.
        /// </summary>
        /// <remarks>
        /// The structure of this JSON depends on the activity type.
        /// </remarks>
        [Required]
        public JsonElement AnswerJson { get; set; }
    }
}
