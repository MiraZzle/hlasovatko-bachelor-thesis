using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

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
    }
}
