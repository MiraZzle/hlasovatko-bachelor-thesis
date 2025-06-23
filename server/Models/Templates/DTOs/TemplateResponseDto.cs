using server.Models.Activities.DTOs;

namespace server.Models.Templates.DTOs
{
    /// <summary>
    /// DTO for returning a full template response.
    /// </summary>
    public class TemplateResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public List<ActivityResponseDto> Definition { get; set; } = new();
    }
}
