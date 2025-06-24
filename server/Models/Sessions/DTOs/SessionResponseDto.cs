using server.Models.Activities.DTOs;
using server.Models.Enums;

namespace server.Models.Sessions.DTOs
{
    public class SessionResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string JoinCode { get; set; } = string.Empty;
        public SessionStatus Status { get; set; }
        public List<ActivityBankResponseDto> Activities { get; set; } = new();
        public DateTime? ActivationDate { get; set; }
        public SessionMode Mode { get; set; }
        public int Participants { get; set; }
        public int? CurrentActivity { get; set; }
    }
}
