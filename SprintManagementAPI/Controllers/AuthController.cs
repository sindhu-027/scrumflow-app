using Microsoft.AspNetCore.Mvc;
using SprintManagementAPI.Services;
using SprintManagementAPI.DTOS;
using SprintManagementAPI.Utils;

namespace SprintManagementAPI.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ================= LOGIN =================
        // POST: api/Users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // ✅ FIX: use PasswordHash (matches FE)
            var user = _authService.Login(loginDto.Email, loginDto.PasswordHash);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            CookieUtil.SetCookie(Response, "sessionId", user.SessionToken, httpOnly: true);

            return Ok(user);
        }

        // ================= REGISTER =================
        // POST: api/Users
        [HttpPost("")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            // ✅ FIX: use PasswordHash (matches FE)
            var user = _authService.Register(
                registerDto.Name,
                registerDto.Email,
                registerDto.PasswordHash
            );

            if (user == null)
                return BadRequest(new { message = "Registration failed. Email may already exist." });

            CookieUtil.SetCookie(Response, "sessionId", user.SessionToken, httpOnly: true);

            return Ok(user);
        }

        // ================= GET CURRENT USER =================
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var sessionId = CookieUtil.GetCookie(Request, "sessionId");

            if (string.IsNullOrEmpty(sessionId))
                return Unauthorized(new { message = "No active session" });

            var user = _authService.GetCurrentUser(sessionId);

            if (user == null)
                return Unauthorized(new { message = "Session expired or invalid" });

            return Ok(user);
        }

        // ================= LOGOUT =================
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var sessionId = CookieUtil.GetCookie(Request, "sessionId");

            if (!string.IsNullOrEmpty(sessionId))
            {
                _authService.Logout(sessionId);
                CookieUtil.DeleteCookie(Response, "sessionId");
            }

            return Ok(new { message = "Logged out successfully" });
        }
    }
}