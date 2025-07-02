using server.Models.Auth;
using server.Models.Auth.DTOs;

namespace server.Services
{
    /// <summary>
    /// Service interface for managing API keys and user auth via API keys.
    /// </summary>
    public interface IApiKeyService
    {
        /// <summary>
        /// Regenerates a new API key for the specified user.
        /// </summary>
        /// <param name="userId">The unique id of the user.</param>
        /// <returns>
        /// A <see cref="NewApiKeyDto"/> containing the new RAW! API key and its metadata.
        /// </returns>
        Task<NewApiKeyDto> RegenerateKeyAsync(Guid userId);

        /// <summary>
        /// Retrieves the partial API key information for a specific user.
        /// </summary>
        /// <param name="userId">The unique id of the user.</param>
        /// <returns>
        /// An <see cref="ApiKeyDto"/> with partial key information, or <c>null</c> if not found.
        /// </returns>
        Task<ApiKeyDto?> GetKeyForUserAsync(Guid userId);

        /// <summary>
        /// Retrieves the user associated with the provided raw API key.
        /// </summary>
        /// <param name="rawApiKey">The raw API key string.</param>
        /// <returns>
        /// The <see cref="User"/> associated with the API key, or <c>null</c> if not found or invalid.
        /// </returns>
        Task<User?> GetUserFromApiKeyAsync(string rawApiKey);
    }
}
