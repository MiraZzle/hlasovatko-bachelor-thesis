using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace server.Models.Answers.DTOs
{
    public class CreateAnswerRequestDto
    {
        [Required]
        public Guid ActivityId { get; set; }

        [Required]
        public Guid ParticipantId { get; set; }

        [Required]
        public JsonElement AnswerJson { get; set; }
    }
}
