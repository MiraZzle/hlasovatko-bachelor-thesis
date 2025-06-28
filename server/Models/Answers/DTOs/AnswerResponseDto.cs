using System.Text.Json;

namespace server.Models.Answers.DTOs
{
    public class AnswerResponseDto
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Guid ParticipantId { get; set; }
        public JsonElement AnswerJson { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
