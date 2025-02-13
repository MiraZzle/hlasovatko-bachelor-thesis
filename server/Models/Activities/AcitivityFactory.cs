﻿using server.Utils;
using System.Text.Json;

namespace server.Models.Activities
{
    public static class ActivityFactory
    {
        private static readonly Dictionary<string, Func<JsonElement, Activity>> _activityRegistry = new();

        // register basic activities
        static ActivityFactory() {
            RegisterActivityType("Quiz", json => CreateQuizActivity(json));
        }

        public static void RegisterActivityType(string type, Func<JsonElement, Activity> creator) {
            _activityRegistry[type] = creator;
        }

        public static Activity CreateActivity(JsonElement activityJson) {
            if (!activityJson.TryGetProperty("type", out var typeProperty)) {
                throw new ArgumentException("Activity JSON must contain a 'type' field.");
            }

            string type = typeProperty.GetString();
            if (_activityRegistry.TryGetValue(type, out var creator)) {
                return creator(activityJson);
            }

            throw new NotSupportedException($"Unsupported activity type: {type}");
        }

        private static Activity CreateQuizActivity(JsonElement json) {
            return new QuizActivity {
                activityName = json.GetProperty("title").GetString(),
                Questions = json.GetProperty("questions").EnumerateArray()
                    .Select(q => new QuizQuestion {
                        Question = q.GetProperty("question").GetString(),
                        Options = q.GetProperty("options").EnumerateArray().Select(o => o.GetString()).ToList(),
                        CorrectOptionIndex = q.GetProperty("correctOptionIndex").GetInt32()
                    }).ToList()
            };
        }
    }
}
