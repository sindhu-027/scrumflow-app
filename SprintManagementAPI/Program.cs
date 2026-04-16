using Microsoft.EntityFrameworkCore;
using SprintManagementAPI.Data;
using SprintManagementAPI.Services;
using SprintManagementAPI.Middlewares;
using System.Text.Json.Serialization; // <-- add this

var builder = WebApplication.CreateBuilder(args);

// ===================== CONTROLLERS =====================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignore reference cycles to prevent JSON serialization errors
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ===================== DATABASE =====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===================== SERVICES =====================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

// ===================== CORS =====================
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ===================== MIDDLEWARE =====================
app.UseHttpsRedirection();
app.UseCors("AllowAngular");

// 🔥 Use AuthMiddleware globally
app.UseAuthMiddleware();

app.UseAuthorization();

// ===================== ENDPOINTS =====================
app.MapControllers();

app.Run();