// using Microsoft.EntityFrameworkCore;
// using SprintManagementAPI.Data;
// using SprintManagementAPI.Services;
// using SprintManagementAPI.Middlewares;
// using System.Text.Json.Serialization;

// var builder = WebApplication.CreateBuilder(args);

// // ===================== ENVIRONMENT =====================
// var env = builder.Environment.EnvironmentName;

// // ===================== CONTROLLERS =====================
// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         // Prevent circular reference issues
//         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//     });

// // ===================== DATABASE =====================
// // Get connection string from appsettings.json / Development / Production
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     options.UseNpgsql(connectionString);
// });

// // ===================== SERVICES =====================
// builder.Services.AddScoped<AuthService>();
// builder.Services.AddScoped<UserService>();

// // ===================== CORS =====================
// var allowedOrigins = builder.Configuration
//     .GetSection("Cors:AllowedOrigins")
//     .Get<string[]>();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAngular", policy =>
//     {
//         policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials();
//     });
// });

// var app = builder.Build();

// // ===================== AUTO MIGRATION =====================
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//     try
//     {
//         db.Database.Migrate();
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine("Database migration failed: " + ex.Message);
//     }
// }

// // ===================== MIDDLEWARE =====================
// app.UseHttpsRedirection();

// app.UseCors("AllowAngular");

// // Custom auth middleware
// app.UseAuthMiddleware();

// app.UseAuthorization();

// // ===================== CONTROLLERS =====================
// app.MapControllers();

// app.Run();



using Microsoft.EntityFrameworkCore;
using SprintManagementAPI.Data;
using SprintManagementAPI.Services;
using SprintManagementAPI.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ===================== CONTROLLERS =====================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ===================== DATABASE =====================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// ===================== SERVICES =====================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

// ===================== CORS (IMPORTANT FIX) =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://scrumflow-app.onrender.com"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ===================== BUILD APP =====================
var app = builder.Build();

// ===================== AUTO MIGRATION =====================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Migration error: " + ex.Message);
    }
}

// ===================== MIDDLEWARE ORDER (VERY IMPORTANT) =====================
app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();