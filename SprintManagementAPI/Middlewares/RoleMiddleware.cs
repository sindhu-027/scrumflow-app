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
            _roles = roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items["User"] is not User user)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Unauthorized" });
                return;
            }

            if (!_roles.Contains(user.Role))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Forbidden: Insufficient role" });
                return;
            }

            await _next(context);
        }
    }

    // Extension method for easy usage
    public static class RoleMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleMiddleware(this IApplicationBuilder builder, string roles)
        {
            return builder.UseMiddleware<RoleMiddleware>(roles);
        }
    }
}