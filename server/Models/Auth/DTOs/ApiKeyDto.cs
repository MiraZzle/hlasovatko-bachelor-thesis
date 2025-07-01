namespace server.Models.Auth.DTOs
{
    /// <summary>
    /// DTO for transfering partial api key information.
    /// </summary>
    public class ApiKeyDto
    {
        /// <summary>
        /// The partial API key for display.
        /// </summary>
        public string PartialKey { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
    }
}
