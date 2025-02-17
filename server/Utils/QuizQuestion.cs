using System.ComponentModel.DataAnnotations;

namespace server.Utils
{
    public class QuizQuestion {
        [Key]
        public Guid QuestionId { get; set; } = Guid.NewGuid();
        public string Question { get; set; } = "";
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectOptionIndex { get; set; }  // specify index of correct option -> may be replaced with correct string
    }
}
