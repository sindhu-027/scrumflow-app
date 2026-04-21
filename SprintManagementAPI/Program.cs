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

// ===================== DATABASE (RAILWAY FINAL FIX) =====================
var databaseUrl = builder.Configuration["DATABASE_URL"];

string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    // 🔥 Parse Railway URL
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    connectionString =
        $"Host={uri.Host};" +
        $"Port={uri.Port};" +
        $"Database={uri.AbsolutePath.TrimStart('/')};" +
        $"Username={userInfo[0]};" +
        $"Password={userInfo[1]};" +
        $"SSL Mode=Require;" +
        $"Trust Server Certificate=true";
}
else
{
    // ✅ Local fallback
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

// 🔥 Register DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ===================== SERVICES =====================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

// ===================== CORS =====================
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAngular", policy =>
//     {
//         policy.WithOrigins(
//                 "http://localhost:4200",
//                 "https://scrumflowapp.onrender.com"
//               )
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials();
//     });
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://scrumflowapp.onrender.com",
                "https://www.scrumflowapp.onrender.com"
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

// ===================== MIDDLEWARE =====================
app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();