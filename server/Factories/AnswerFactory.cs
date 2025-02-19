using System;
using System.Collections.Generic;
using System.Text.Json;
using server.Models.Answers;

namespace server.Utils
{
    public static class AnswerFactory
    {
        private static readonly Dictionary<string, Func<JsonElement, Answer>> _answerRegistry = new();

        static AnswerFactory() {
            RegisterAnswerType("Quiz", CreateQuizAnswer);
        }

        public static void RegisterAnswerType(string type, Func<JsonElement, Answer> creator) {
            _answerRegistry[type] = creator;
        }

        public static Answer CreateAnswer(JsonElement answerJson) {
            if (!answerJson.TryGetProperty("AnswerType", out var typeProperty)) {
                throw new ArgumentException("Answer JSON must contain an 'AnswerType' field.");
            }

            string type = typeProperty.GetString();
            if (_answerRegistry.TryGetValue(type, out var creator)) {
                return creator(answerJson);
            }

            throw new NotSupportedException($"Unsupported answer type: {type}");
        }

        private static Answer CreateQuizAnswer(JsonElement json) {
            return new QuizAnswer {
                SessionId = Guid.Parse(json.GetProperty("sessionId").GetString()),
                ActivityId = Guid.Parse(json.GetProperty("activityId").GetString()),
                QuestionId = Guid.Parse(json.GetProperty("questionId").GetString()),
                SelectedOption = json.GetProperty("selectedOption").GetString()
            };
        }
    }
}
