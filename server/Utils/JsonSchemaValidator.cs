using Microsoft.Extensions.Logging;
using NJsonSchema;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace server.Utils
{
    /// <summary>
    /// Provides functionality to validate JSON data against a specified JSON schema.
    /// </summary>
    public static class JsonSchemaValidator
    {
        /// <summary>
        /// Validates a JSON string against a JSON schema file.
        /// </summary>
        /// <param name="json">The JSON string to validate.</param>
        /// <param name="schemaPath">The file path to the JSON schema.</param>
        /// <param name="validationTarget">A label describing what is being validated (used in logs and error messages).</param>
        /// <param name="logger">The logger for recording validation errors and issues.</param>
        public static async Task ValidateAsync(string json, string schemaPath, string validationTarget, ILogger logger) {
            if (!File.Exists(schemaPath)) {
                logger.LogError("Schema file not found at path: {SchemaPath}", schemaPath);
                throw new FileNotFoundException($"Schema file for {validationTarget} not found.", schemaPath);
            }

            var schema = await JsonSchema.FromFileAsync(schemaPath);
            var errors = schema.Validate(json);

            // Log validation errors if any
            if (errors.Any()) {
                var errorMessages = string.Join(", ", errors.Select(e => $"{e.Path}: {e.Kind}"));
                logger.LogWarning("JSON validation failed for {ValidationTarget}. Errors: {Errors}", validationTarget, errorMessages);
                throw new JsonException($"Invalid {validationTarget}: {errorMessages}");
            }
        }
    }
}
