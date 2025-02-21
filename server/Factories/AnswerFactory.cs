using System;
using System.Collections.Generic;
using System.Text.Json;
using server.Models.Answers;

namespace server.Utils
{
    public static class AnswerFactory
    {
        private static readonly Dictionary<string, Func<JsonElement, List<Answer>>> _answerRegistry = new();

        static AnswerFactory() {
            RegisterAnswerType("Quiz", CreateQuizAnswers);
        }

        public static void RegisterAnswerType(string type, Func<JsonElement, List<Answer>> creator) {
            _answerRegistry[type] = creator;
        }

        public static List<Answer> CreateAnswer(JsonElement answerJson) {
            if (!answerJson.TryGetProperty("answerType", out var typeProperty)) {
                throw new ArgumentException("Answer JSON must contain an 'answerType' field.");
            }

            string type = typeProperty.GetString();
            if (_answerRegistry.TryGetValue(type, out var creator)) {
                return creator(answerJson);
            }

            throw new NotSupportedException($"Unsupported answer type: {type}");
        }


        private static List<Answer> CreateQuizAnswers(JsonElement json) {
            var sessionId = Guid.Parse(json.GetProperty("sessionId").GetString());
            var activityId = Guid.Parse(json.GetProperty("activityId").GetString());

            var answers = new List<Answer>();

            // property 'answers' is set -> multiple answers
            if (json.TryGetProperty("answers", out var answersJson) && answersJson.ValueKind == JsonValueKind.Array) {
                foreach (var answer in answersJson.EnumerateArray()) {
                    answers.Add(CreateSingleQuizAnswer(sessionId, activityId, answer));
                }
            } else {
                answers.Add(CreateSingleQuizAnswer(sessionId, activityId, json));
            }

            return answers;
        }

        private static Answer CreateSingleQuizAnswer(Guid sessionId, Guid activityId, JsonElement answerJson) {
            var questionId = Guid.Parse(answerJson.GetProperty("questionId").GetString());
            var selectedOption = answerJson.GetProperty("selectedOption").GetString();

            return new QuizAnswer {
                SessionId = sessionId,
                ActivityId = activityId,
                QuestionId = questionId,
                SelectedOption = selectedOption
            };
        }
    }
}
