using server.Models.Statistics.DTOs;

namespace server.Services
{
    /// <summary>
    /// Defines the contract for the statistics service.
    /// </summary>
    public interface IStatisticsService
    {
        /// <summary>
        /// Computes and gets statistics for a given user.
        /// </summary>
        /// <param name="userId"> The ID of the user.</param>
        /// <returns>A StatisticsDto object containing the computed statistics.</returns>
        Task<StatisticsDto> GetStatistics(Guid userId);

        /// <summary>
        /// Returns the statistics for a given user in CSV format.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A string containing the statistics in CSV format.</returns>
        Task<string> GetStatisticsCsv(Guid userId);

        /// <summary>
        /// Returns the statistics for a given user in JSON format.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A string containing the statistics in JSON format.</returns>
        Task<string> GetStatisticsJson(Guid userId);
    }
}
