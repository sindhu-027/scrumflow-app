using Microsoft.AspNetCore.Http;

namespace SprintManagementAPI.Utils
{
    public static class CookieUtil
    {
        private const int DefaultCookieExpiryDays = 7; // 1 week

        // ✅ Set cookie
        public static void SetCookie(HttpResponse response, string key, string? value, int? expireDays = null, bool httpOnly = true)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cookie key cannot be null or empty", nameof(key));

            var options = new CookieOptions
            {
                HttpOnly = httpOnly,
                Secure = false, // set true in production (HTTPS)
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(expireDays ?? DefaultCookieExpiryDays)
            };

            response.Cookies.Append(key, value ?? "", options);
        }

        // ✅ Get cookie
        public static string? GetCookie(HttpRequest request, string key)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(key)) return null;

            return request.Cookies.TryGetValue(key, out var value) ? value : null;
        }

        // ✅ Delete cookie
        public static void DeleteCookie(HttpResponse response, string key)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (string.IsNullOrEmpty(key)) return;

            response.Cookies.Delete(key);
        }
    }
}