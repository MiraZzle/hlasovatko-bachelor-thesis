# EngaGenie - Server

The server is built with ASP.NET Core and exposes a REST API for managing users, sessions, activities, and responses.

## API

The server exposes a REST API documented with Swagger. When run with Docker, the documentation is available at:

- **Backend API:** [Swagger](http://localhost/server/swagger/index.html)

## Extending the System – Adding a New Activity Type

The backend supports dynamic activity types. To add a new one, follow these steps:

### 1. Create JSON Schemas

Define the structure of the activity and its answers using two JSON schema files:

- `server/Schemas/Activities/new_activity.json` – defines the activity format
- `server/Schemas/Answers/new_activity_answer.json` – defines the expected answer format

### 2. Implement a Result Processor

Create a class in `server/Services/Analytics/Processors/`, e.g.:

```csharp
public class NewActivityResultProcessor : IActivityResultProcessor
{
    public string ActivityType => "new_activity";

    public ActivityResultDto Process(IEnumerable<Answer> answers)
    {
        // Custom logic for processing answers
        return new ActivityResultDto
        {
            ActivityType = this.ActivityType,
            Result = new { /* your result */ }
        };
    }
}
```

### 3. Register the Processor

In `Program.cs`, register the new processor:

```csharp
builder.Services.AddScoped<IActivityResultProcessor, NewActivityResultProcessor>();
```

## Testing

This project uses [xUnit](https://xunit.net/) for Server unit testing.

> [!WARNING]
> You must have the [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/) installed to run tests from the command line.

To run all tests, navigate to `server.Tests` directory and use:

```bash
dotnet test
```
