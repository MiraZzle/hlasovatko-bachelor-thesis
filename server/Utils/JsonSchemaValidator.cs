using Microsoft.Extensions.Logging;
using NJsonSchema;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace server.Utils
{
    public static class JsonSchemaValidator
    {
        public static async Task ValidateAsync(string json, string schemaPath, string validationTarget, ILogger logger) {
            if (!File.Exists(schemaPath)) {
                logger.LogError("Schema file not found at path: {SchemaPath}", schemaPath);
                throw new FileNotFoundException($"Schema file for {validationTarget} not found.", schemaPath);
            }

            var schema = await JsonSchema.FromFileAsync(schemaPath);
            var errors = schema.Validate(json);

            if (errors.Any()) {
                var errorMessages = string.Join(", ", errors.Select(e => $"{e.Path}: {e.Kind}"));
                logger.LogWarning("JSON validation failed for {ValidationTarget}. Errors: {Errors}", validationTarget, errorMessages);
                throw new JsonException($"Invalid {validationTarget}: {errorMessages}");
            }
        }
    }
}
