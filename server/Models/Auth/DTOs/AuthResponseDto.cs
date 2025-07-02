namespace server.Models.Auth.DTOs
{
    /// <summary>
    /// DTO for returning authentication results to the client.
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// The unique identifier of the authenticated user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// The display name of the authenticated user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The email address of the authenticated user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The JWT token issued for the authenticated user.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
