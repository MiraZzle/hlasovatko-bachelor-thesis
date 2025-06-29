namespace server.Models.Statistics.DTOs
{
    public class StatisticsDto
    {
        public int TotalSessions { get; set; }
        public int TotalActivities { get; set; }
        public string MostCommonActivityType { get; set; } = "N/A";
    }
}
