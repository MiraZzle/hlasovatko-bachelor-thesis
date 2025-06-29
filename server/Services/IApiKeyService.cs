using server.Models.Auth;
using server.Models.Auth.DTOs;

namespace server.Services
{
    public interface IApiKeyService
    {
        Task<NewApiKeyDto> RegenerateKeyAsync(Guid userId);
        Task<ApiKeyDto?> GetKeyForUserAsync(Guid userId);
        Task<User?> GetUserFromApiKeyAsync(string rawApiKey);
    }
}
