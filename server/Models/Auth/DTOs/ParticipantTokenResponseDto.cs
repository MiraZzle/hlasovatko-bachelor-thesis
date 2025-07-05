namespace server.Models.Auth.DTOs
{
    /// <summary>
    /// DTO for returning a participant token response.
    /// </summary>
    public class ParticipantTokenResponseDto
    {
        /// <summary>
        /// A short-lived JSON Web Token for participants.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
