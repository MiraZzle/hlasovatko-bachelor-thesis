using server.Models.Auth;
using server.Models.Auth.DTOs;

namespace server.Services
{
    /// <summary>
    /// Service interface for user authentication and acc management.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="request">The registration request containing user info.</param>
        /// <returns>The created <see cref="User"/> entity.</returns>
        Task<User> RegisterAsync(RegisterRequestDto request);

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>An <see cref="AuthResponseDto"/> containing authentication result and JWT token.</returns>
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

        /// <summary>
        /// Changes the password for the specified user.
        /// </summary>
        /// <param name="userId">The unique id of the user.</param>
        /// <param name="request">The password change request containing old and new passwords.</param>
        /// <returns><c>true</c> if the password was changed successfully; otherwise, <c>false</c>.</returns>
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);

        /// <summary>
        /// Verifies a session join code and generates a short-lived JWT for a participant.
        /// </summary>
        /// <param name="request">The request containing the join code.</param>
        /// <returns>A DTO containing the participants token, or null if the join code is invalid.</returns>
        Task<ParticipantTokenResponseDto?> VerifyParticipantAsync(ValidateParticipantRequestDto request);
    }
}
