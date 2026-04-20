using Microsoft.AspNetCore.Http;
using SprintManagementAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SprintManagementAPI.Middlewares
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _roles;

        public RoleMiddleware(RequestDelegate next, string roles)
        {
            _next = next;

            // ✅ handle null/empty safely
            _roles = roles?
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? Array.Empty<string>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // ✅ check user from AuthMiddleware
            if (context.Items["User"] is not User user)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Unauthorized" });
                return;
            }

            // ✅ case-insensitive role check (IMPORTANT FIX)
            if (!_roles.Any(r => r.Equals(user.Role, StringComparison.OrdinalIgnoreCase)))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Forbidden: Insufficient role" });
                return;
            }

            await _next(context);
        }
    }

    // Extension method
    public static class RoleMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleMiddleware(this IApplicationBuilder builder, string roles)
        {
            return builder.UseMiddleware<RoleMiddleware>(roles);
        }
    }
}