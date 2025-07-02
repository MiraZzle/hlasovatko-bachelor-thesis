using System.Text.Json;

namespace server.Models.Answers.DTOs
{
    /// <summary>
    /// DTO for returning an individual answer to an activity.
    /// </summary>
    public class AnswerResponseDto
    {
        /// <summary>
        /// The unique identifier of the answer.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the activity this answer is for.
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// The unique identifier of the participant who submitted the answer.
        /// </summary>
        public Guid ParticipantId { get; set; }

        /// <summary>
        /// The answer content as a JSON object.
        /// </summary>
        public JsonElement AnswerJson { get; set; }

        /// <summary>
        /// The timestamp when the answer was submitted.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
