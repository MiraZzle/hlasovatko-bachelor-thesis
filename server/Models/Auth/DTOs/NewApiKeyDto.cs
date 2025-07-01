namespace server.Models.Auth.DTOs
{
    /// <summary>
    /// DTO for creating a new API key.
    /// </summary>
    public class NewApiKeyDto
    {
        /// <summary>
        /// Raw API key string to be created.
        /// </summary>
        public string RawApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Metadata about the API key being created.
        /// </summary>
        public ApiKeyDto KeyInfo { get; set; } = new();
    }
}
