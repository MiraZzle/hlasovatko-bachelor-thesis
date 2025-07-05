using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Auth;
using server.Data;
using server.Services;
using server.Services.Analytics;
using server.Services.Analytics.Processors;
using server.Services.Background;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace server
{
    public class Program
    {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Setup auth
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            }).AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(
                ApiKeyAuthOptions.DefaultScheme, options => {});

            builder.Services.AddAuthorization(options => {
                options.AddPolicy("AuthenticatedUser", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthOptions.DefaultScheme)
                    .RequireAuthenticatedUser()
                    .Build());

                options.AddPolicy("Participant", new AuthorizationPolicyBuilder()
                   .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                   .RequireAuthenticatedUser()
                   .RequireClaim(ClaimTypes.Role, "Participant")
                   .RequireClaim("sessionId")
                   .RequireClaim("participantId")
                   .Build());

                options.AddPolicy("SessionViewer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Role, "User", "Participant")
                    .Build());
            });

            // Get DB connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                 options.UseNpgsql(connectionString, npgsqlOptions => {
                     npgsqlOptions.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: TimeSpan.FromSeconds(10),
                         errorCodesToAdd: null);
                 }));

            // Setup CORS to allow frontend clients
            var clientUrl = builder.Configuration["CLIENT_URL"] ?? "http://localhost:3000";
            var managerUrl = builder.Configuration["MANAGER_URL"] ?? "http://localhost:3001";

            builder.Services.AddCors(options => {
                options.AddDefaultPolicy(policy => {
                    policy.WithOrigins(clientUrl, managerUrl)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Register custom services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<ITemplateService, TemplateService>();
            builder.Services.AddScoped<IAnswerService, AnswerService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();

            // Register analytics processor
            builder.Services.AddScoped<IAnalyticsProcessor, AnalyticsProcessor>();
            builder.Services.AddScoped<IActivityResultProcessor, PollResultProcessor>();
            builder.Services.AddScoped<IActivityResultProcessor, MultipleChoiceResultProcessor>();
            builder.Services.AddScoped<IActivityResultProcessor, OpenEndedResultProcessor>();
            builder.Services.AddScoped<IActivityResultProcessor, ScaleRatingResultProcessor>();

            // Background services
            builder.Services.AddHostedService<SessionActivationService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            var app = builder.Build();

            // Automatically apply migrations on startup
            try {
                using (var scope = app.Services.CreateScope()) {
                    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
                    using (var dbContext = dbContextFactory.CreateDbContext()) {
                        dbContext.Database.Migrate();
                    }
                }

                // Seed initial data - for demo
                await DataSeeder.SeedAdminUserAsync(app);
            }
            catch (Exception ex) {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }

            // Middleware
            app.UseCors();
            app.UseSwagger();
            app.UseSwaggerUI();

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
