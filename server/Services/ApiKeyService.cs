using server.Data;
using server.Models.Auth;
using server.Models.Auth.DTOs;
using server.Models.Auth.server.Models.Auth;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace server.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly AppDbContext _context;

        public ApiKeyService(AppDbContext context) {
            _context = context;
        }

        public async Task<NewApiKeyDto> RegenerateKeyAsync(Guid userId) {
            var user = await _context.Users.Include(u => u.ApiKey).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) {
                throw new Exception("User not found.");
            }

            // if old key exists, remove it
            if (user.ApiKey != null) {
                _context.ApiKeys.Remove(user.ApiKey);
            }

            var rawApiKey = GenerateSecureApiKey();
            var newApiKey = new ApiKey {
                UserId = userId,
                KeyHash = BCrypt.Net.BCrypt.HashPassword(rawApiKey),
                // Show only the first 7 characters for info
                PartialKey = rawApiKey.Substring(0, 7) + "...",
                CreatedDate = DateTime.UtcNow
            };

            _context.ApiKeys.Add(newApiKey);
            await _context.SaveChangesAsync();

            return new NewApiKeyDto {
                RawApiKey = rawApiKey,
                KeyInfo = new ApiKeyDto {
                    PartialKey = newApiKey.PartialKey,
                    CreatedDate = newApiKey.CreatedDate,
                    LastUsedDate = newApiKey.LastUsedDate
                }
            };
        }

        public async Task<ApiKeyDto?> GetKeyForUserAsync(Guid userId) {
            var apiKey = await _context.ApiKeys
                .AsNoTracking()
                .Where(k => k.UserId == userId)
                .Select(k => new ApiKeyDto {
                    PartialKey = k.PartialKey,
                    CreatedDate = k.CreatedDate,
                    LastUsedDate = k.LastUsedDate
                })
                .FirstOrDefaultAsync();

            return apiKey;
        }

        public async Task<User?> GetUserFromApiKeyAsync(string rawApiKey) {
            // Linear search through all keys to find a match
            // Could be improved - todo later
            var allKeys = await _context.ApiKeys.Include(k => k.User).ToListAsync();

            foreach (var key in allKeys) {
                if (BCrypt.Net.BCrypt.Verify(rawApiKey, key.KeyHash)) {
                    key.LastUsedDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return key.User;
                }
            }

            return null;
        }

        private string GenerateSecureApiKey() {
            const int keyLength = 32;
            var bytes = new byte[keyLength];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(bytes);
            }
            // url safe base64 encoding without padding
            return Convert.ToBase64String(bytes)
                .Replace("/", "_")
                .Replace("+", "-")
                .TrimEnd('=');
        }
    }
}
