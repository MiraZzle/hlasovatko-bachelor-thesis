using System;

namespace server.Models.Answers
{
    public class QuizAnswer : Answer
    {
        public override string AnswerType => "Quiz";
        public Guid QuestionId { get; set; }
        public string SelectedOption { get; set; }
    }
}
