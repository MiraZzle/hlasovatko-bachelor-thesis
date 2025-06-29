using server.Models.Auth;
using server.Models.Auth.DTOs;

namespace server.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);

    }
}
