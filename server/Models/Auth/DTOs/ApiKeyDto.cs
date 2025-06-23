namespace server.Models.Auth.DTOs
{
    public class ApiKeyDto
    {
        public string PartialKey { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
    }
}
