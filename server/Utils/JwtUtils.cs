using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.Models.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server.Utils
{
    /// <summary>
    /// Utility class for generating JWT tokens for users.
    /// </summary>
    public static class JwtUtils
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        public static string GenerateUserToken(User user, IConfiguration configuration) {
            var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
            var jwtIssuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a JWT token for anonymous session participant.
        /// </summary>
        public static string GenerateParticipantToken(Guid sessionId, Guid participantId, IConfiguration configuration) {
            var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
            var jwtIssuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("sessionId", sessionId.ToString()),
                new Claim("participantId", participantId.ToString()),
                new Claim(ClaimTypes.Role, "Participant"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
