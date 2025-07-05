using BCrypt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.Models.Auth;
using server.Models.Auth.DTOs;
using server.Models.Enums;
using server.Utils;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(RegisterRequestDto request) {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email)) {
                throw new Exception("User with this email already exists.");
            }

            var user = new User {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) {
                throw new Exception("Invalid credentials.");
            }

            var token = JwtUtils.GenerateUserToken(user, _configuration);

            return new AuthResponseDto {
                UserId = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request) {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash)) {
                throw new Exception("Incorrect old password.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ParticipantTokenResponseDto?> VerifyParticipantAsync(ValidateParticipantRequestDto request) {
            var session = await _context.Sessions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.SessionId && s.JoinCode == request.JoinCode);

            if (session == null || session.Status != SessionStatus.Active) {
                return null;
            }

            var participantId = Guid.NewGuid();

            var tokenString = JwtUtils.GenerateParticipantToken(session.Id, participantId, _configuration);

            return new ParticipantTokenResponseDto {
                Token = tokenString
            };
        }
    }
}
