using Microsoft.AspNetCore.Http;
using SprintManagementAPI.Services;
using SprintManagementAPI.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

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
            try
            {
                // ===================== GET TOKEN =====================
                var sessionToken = CookieUtil.GetCookie(context.Request, "sessionId");

                // fallback: Authorization header
                if (string.IsNullOrEmpty(sessionToken))
                {
                    sessionToken = context.Request.Headers["Authorization"]
                        .FirstOrDefault()?.Replace("Bearer ", "");
                }

                // ===================== VALIDATE USER =====================
                if (!string.IsNullOrEmpty(sessionToken))
                {
                    var user = authService.GetCurrentUser(sessionToken);

                    if (user != null)
                    {
                        context.Items["User"] = user;
                    }
                }
            }
            catch (Exception ex)
            {
                // 🔥 IMPORTANT: NEVER break pipeline (prevents CORS failure)
                Console.WriteLine("AuthMiddleware Error: " + ex.Message);
            }

            // ===================== CONTINUE PIPELINE =====================
            await _next(context);
        }
    }

    // ===================== EXTENSION =====================
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}




















// using Microsoft.AspNetCore.Http;
// using SprintManagementAPI.Services;
// using SprintManagementAPI.Utils;
// using System.Threading.Tasks;

// namespace SprintManagementAPI.Middlewares
// {
//     public class AuthMiddleware
//     {
//         private readonly RequestDelegate _next;

//         public AuthMiddleware(RequestDelegate next)
//         {
//             _next = next;
//         }

//         public async Task InvokeAsync(HttpContext context, AuthService authService)
//         {
//             // ✅ Try cookie first
//             var sessionToken = CookieUtil.GetCookie(context.Request, "sessionId");

//             // 🔥 FALLBACK: also support header (important for production/debug)
//             if (string.IsNullOrEmpty(sessionToken))
//             {
//                 sessionToken = context.Request.Headers["Authorization"]
//                     .FirstOrDefault()?.Replace("Bearer ", "");
//             }

//             if (!string.IsNullOrEmpty(sessionToken))
//             {
//                 var user = authService.GetCurrentUser(sessionToken);

//                 if (user != null)
//                 {
//                     context.Items["User"] = user;
//                 }
//             }

//             await _next(context);
//         }
//     }

//     public static class AuthMiddlewareExtensions
//     {
//         public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
//         {
//             return builder.UseMiddleware<AuthMiddleware>();
//         }
//     }
// }