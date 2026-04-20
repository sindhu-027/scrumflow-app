using SprintManagementAPI.Models;
using SprintManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace SprintManagementAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // ================= LOGIN =================
        public User? Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            email = email.Trim().ToLower();
            password = password.Trim();

            var user = _context.Users
                .FirstOrDefault(u => u.Email.ToLower() == email);

            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isValid)
                return null;

            // 🔥 regenerate session token
            user.SessionToken = Guid.NewGuid().ToString();

            _context.SaveChanges();

            return user;
        }

        // ================= REGISTER =================
        public User? Register(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return null;

            name = name.Trim();
            email = email.Trim().ToLower();
            password = password.Trim();

            // 🔥 prevent duplicate (case-insensitive)
            if (_context.Users.Any(u => u.Email.ToLower() == email))
                return null;

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "Developer",
                SessionToken = Guid.NewGuid().ToString()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        // ================= CURRENT USER =================
        public User? GetCurrentUser(string sessionToken)
        {
            if (string.IsNullOrWhiteSpace(sessionToken))
                return null;

            return _context.Users
                .FirstOrDefault(u => u.SessionToken == sessionToken);
        }

        // ================= LOGOUT =================
        public void Logout(string sessionToken)
        {
            if (string.IsNullOrWhiteSpace(sessionToken))
                return;

            var user = _context.Users
                .FirstOrDefault(u => u.SessionToken == sessionToken);

            if (user != null)
            {
                user.SessionToken = null;
                _context.SaveChanges();
            }
        }
    }
}