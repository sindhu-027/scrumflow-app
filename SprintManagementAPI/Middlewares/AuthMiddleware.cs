using Microsoft.AspNetCore.Http;
using SprintManagementAPI.Services;
using SprintManagementAPI.Utils;
using System.Threading.Tasks;

namespace SprintManagementAPI.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthService authService)
        {
            // ✅ Get session token from cookie
            var sessionToken = CookieUtil.GetCookie(context.Request, "sessionId");

            if (!string.IsNullOrEmpty(sessionToken))
            {
                var user = authService.GetCurrentUser(sessionToken);
                if (user != null)
                {
                    // Attach user to context.Items
                    context.Items["User"] = user;
                }
            }

            await _next(context);
        }
    }

    // Extension method to add middleware easily
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}