using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using server.Extensions;

namespace server.Controllers
{
    /// <summary>
    /// Controller for any kind of stats.
    /// Requires authenticated user access.
    /// </summary>
    [ApiController]
    [Route("api/v1/statistics")]
    [Authorize(Policy = "AuthenticatedUser")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService) {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Retrieves overall statistics for the authenticated user.
        /// </summary>
        /// <returns>
        /// 200 OK with a StatisticsDto obj containing the users statistics.
        /// </returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetStatistics() {
            var userId = this.GetCurrentUserId();
            var stats = await _statisticsService.GetStatistics(userId);
            return Ok(stats);
        }

        /// <summary>
        /// Exports the users statistics in specified format.
        /// </summary>
        /// <param name="format">
        /// The export format: <c>csv</c> or <c>json</c> (case-insensitive).
        /// </param>
        /// <returns>
        /// 200 OK with a file containing the statistics in the requested format.<br/>
        /// 400 Bad Request if the format is invalid.
        /// </returns>
        [HttpGet("export")]
        public async Task<IActionResult> ExportStatistics([FromQuery] string format) {
            var userId = this.GetCurrentUserId();
            var formatLower = format?.ToLowerInvariant();

            // decide the format based on the query param
            switch (formatLower) {
                case "csv": {
                    var csvData = await _statisticsService.GetStatisticsCsv(userId);
                    var fileName = $"engagenie-statistics-{DateTime.UtcNow:yyyy-MM-dd}.csv";
                    return File(System.Text.Encoding.UTF8.GetBytes(csvData), "text/csv", fileName);
                }
                case "json": {
                    var jsonData = await _statisticsService.GetStatisticsJson(userId);
                    var fileName = $"engagenie-statistics-{DateTime.UtcNow:yyyy-MM-dd}.json";
                    return File(System.Text.Encoding.UTF8.GetBytes(jsonData), "application/json", fileName);
                }
                default:
                    return BadRequest(new { message = "Invalid format specified. Please use 'csv' or 'json'." });
            }
        }
    }
}
