namespace server.Models.Statistics.DTOs
{
    /// <summary>
    /// DTO for returning users statistics about sessions and activities.
    /// </summary>
    public class StatisticsDto
    {
        /// <summary>
        /// The total number of sessions created by the user.
        /// </summary>
        public int TotalSessions { get; set; }


        /// <summary>
        /// The total number of activities across all sessions for the user.
        /// </summary>
        public int TotalActivities { get; set; }

        /// <summary>
        /// The most frequently used activity type by the user, or "N/A" if not available.
        /// </summary>
        public string MostCommonActivityType { get; set; } = "N/A";
    }
}
