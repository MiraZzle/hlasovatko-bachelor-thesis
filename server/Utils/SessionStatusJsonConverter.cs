using System.Text.Json;
using System.Text.Json.Serialization;
using server.Models.Enums;

namespace server.Utils
{
    public class SessionStatusJsonConverter : JsonConverter<SessionStatus>
    {
        public override SessionStatus Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) {
            var value = reader.GetString()?.ToLowerInvariant();
            return value switch {
                "planned" => SessionStatus.Planned,
                "active" => SessionStatus.Active,
                "inactive" => SessionStatus.Inactive,
                "finished" => SessionStatus.Finished,
                _ => throw new JsonException($"Unable to convert '{value}' to SessionStatus.")
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            SessionStatus value,
            JsonSerializerOptions options) {
            var stringValue = value switch {
                SessionStatus.Planned => "planned",
                SessionStatus.Active => "active",
                SessionStatus.Inactive => "inactive",
                SessionStatus.Finished => "finished",
                _ => value.ToString().ToLowerInvariant()
            };
            writer.WriteStringValue(stringValue);
        }
    }
}
