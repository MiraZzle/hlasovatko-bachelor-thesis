using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace server.Models.Answers
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "AnswerType")]
    [JsonDerivedType(typeof(QuizAnswer), "Quiz")]
    public abstract class Answer
    {
        [Key]
        public Guid AnswerId { get; set; } = Guid.NewGuid();
        public Guid SessionId { get; set; }
        public Guid ActivityId { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public virtual string AnswerType { get; }
    }
}
