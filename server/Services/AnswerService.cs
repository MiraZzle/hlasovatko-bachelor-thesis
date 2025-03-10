﻿using System.Text.Json;
using server.Data;
using server.Models.Answers;
using Microsoft.EntityFrameworkCore;
using server.Utils;

namespace server.Services
{
    public class AnswerService
    {
        private readonly AppDbContext _context;

        public AnswerService(AppDbContext context) {
            _context = context;
        }

        public async Task<bool> SubmitAnswerAsync(JsonElement answerJson) {
            var answers = AnswerFactory.CreateAnswer(answerJson);
            if (answers == null || answers.Count == 0)
                return false;

            _context.Answers.AddRange(answers);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Answer>> GetSessionAnswersAsync(Guid sessionId) {
            return await _context.Answers
                .Where(a => a.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<List<Answer>> GetActivityAnswersAsync(Guid activityId) {
            return await _context.Answers
                .Where(a => a.ActivityId == activityId)
                .ToListAsync();
        }
    }
}
