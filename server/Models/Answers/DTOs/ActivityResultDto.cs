using server.Models.Activities.DTOs;
using System.Text.Json;

namespace server.Models.Answers.DTOs
{
    public class ActivityResultDto
    {
        public ActivityResponseDto ActivityRef { get; set; }
        public JsonElement Results { get; set; }
    }
}
