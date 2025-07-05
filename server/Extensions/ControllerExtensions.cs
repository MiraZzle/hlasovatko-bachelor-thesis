using Microsoft.AspNetCore.Mvc;
using server.Data;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace server.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Gets the current authenticated user's ID from the JWT token claims.
        /// </summary>
        public static Guid GetCurrentUserId(this ControllerBase controller) {
            var userIdClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) {
                throw new InvalidOperationException("User ID could not be determined from the token.");
            }
            return userId;
        }

        /// <summary>
        /// Checks if the current user is authorized to view a specific session,
        /// either as the session owner or as a participant in that session.
        /// </summary>
        /// <param name="controller">The controller instance.</param>
        /// <param name="sessionId">The ID of the session to check.</param>
        /// <param name="context">The database context for the query.</param>
        /// <returns>True if the user is authorized, otherwise false.</returns>
        public static async Task<bool> IsAuthorizedForSessionAsync(this ControllerBase controller, Guid sessionId, AppDbContext context) {
            if (controller.User.IsInRole("Participant")) {
                var sessionIdFromToken = controller.User.FindFirst("sessionId")?.Value;
                return sessionId.ToString() == sessionIdFromToken;
            }

            if (controller.User.IsInRole("User")) {
                var userIdFromToken = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdFromToken, out var ownerId)) {
                    return await context.Sessions
                        .AnyAsync(s => s.Id == sessionId && s.Template.OwnerId == ownerId);
                }
            }

            return false;
        }
    }
}
