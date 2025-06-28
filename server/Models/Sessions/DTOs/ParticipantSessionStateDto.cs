using server.Models.Enums;

namespace server.Models.Sessions.DTOs
{
    public class ParticipantSessionStateDto
    {
        public Guid SessionId { get; set; }
        public SessionStatus Status { get; set; }
        public Guid? CurrentActivityId { get; set; }
        public bool ShowResults { get; set; }
    }
}
