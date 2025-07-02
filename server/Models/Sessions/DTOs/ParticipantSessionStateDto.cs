using server.Models.Enums;

namespace server.Models.Sessions.DTOs
{
    /// <summary>
    /// DTO representing the current state of a session for a participant.
    /// </summary>
    public class ParticipantSessionStateDto
    {
        /// <summary>
        /// The unique identifier of the session.
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// The current status of the session.
        /// </summary>
        public SessionStatus Status { get; set; }

        /// <summary>
        /// The unique identifier of the current activity in the session, or null if no activity is active.
        /// </summary>
        public Guid? CurrentActivityId { get; set; }

        /// <summary>
        /// Indicates whether the results are currently visible to the participant.
        /// </summary>
        public bool ShowResults { get; set; }
    }
}
