using System.Text.Json;
using System.Text.Json.Serialization;
using server.Models.Enums;

namespace server.Utils
{    
    /// <summary>
    /// My custom JSON converter for the SessionMode enum.
    /// Serializes <see cref="SessionMode"/> values as kebab-case strings 
    /// ("teacher-paced" or "student-paced") and deserializes them back to the corresponding enum values.
    /// </summary>
    public class SessionModeJsonConverter : JsonConverter<SessionMode>
    {
        public override SessionMode Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) {
            var value = reader.GetString();
            return value switch {
                "teacher-paced" => SessionMode.TeacherPaced,
                "student-paced" => SessionMode.StudentPaced,
                _ => throw new JsonException($"Unable to convert '{value}' to SessionMode.")
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            SessionMode value,
            JsonSerializerOptions options) {
            var stringValue = value switch {
                SessionMode.TeacherPaced => "teacher-paced",
                SessionMode.StudentPaced => "student-paced",
                _ => value.ToString()
            };
            writer.WriteStringValue(stringValue);
        }
    }
}
