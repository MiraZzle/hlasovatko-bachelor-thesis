namespace server.Models.Auth.DTOs
{
    public class NewApiKeyDto
    {
        public string RawApiKey { get; set; } = string.Empty;
        public ApiKeyDto KeyInfo { get; set; } = new();
    }
}
