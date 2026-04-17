// using SprintManagementAPI.Models;
// using SprintManagementAPI.Data;
// using Microsoft.EntityFrameworkCore;

// namespace SprintManagementAPI.Services
// {
//     public class AuthService
//     {
//         private readonly AppDbContext _context;

//         public AuthService(AppDbContext context)
//         {
//             _context = context;
//         }

//         // ✅ LOGIN
//         public User? Login(string email, string password)
//         {
//             if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
//                 return null;

//             var user = _context.Users.FirstOrDefault(u => u.Email == email);

//             if (user == null)
//                 return null;

//             // 🔥 Verify hashed password
//             bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

//             if (!isValid)
//                 return null;

//             // 🔥 Generate new session token
//             user.SessionToken = Guid.NewGuid().ToString();

//             _context.SaveChanges();

//             return user;
//         }

//         // ✅ REGISTER
//         public User? Register(string name, string email, string password)
//         {
//             if (string.IsNullOrWhiteSpace(name) ||
//                 string.IsNullOrWhiteSpace(email) ||
//                 string.IsNullOrWhiteSpace(password))
//                 return null;

//             // 🔥 Prevent duplicate users
//             if (_context.Users.Any(u => u.Email == email))
//                 return null;

//             // 🔥 Hash password
//             string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

//             var user = new User
//             {
//                 Name = name.Trim(),
//                 Email = email.Trim().ToLower(),
//                 PasswordHash = hashedPassword,
//                 Role = "Developer",
//                 SessionToken = Guid.NewGuid().ToString()
//             };

//             _context.Users.Add(user);
//             _context.SaveChanges();

//             return user;
//         }

//         // ✅ GET CURRENT USER
//         public User? GetCurrentUser(string sessionToken)
//         {
//             if (string.IsNullOrEmpty(sessionToken))
//                 return null;

//             return _context.Users
//                 .FirstOrDefault(u => u.SessionToken == sessionToken);
//         }

//         // ✅ LOGOUT
//         public void Logout(string sessionToken)
//         {
//             if (string.IsNullOrEmpty(sessionToken))
//                 return;

//             var user = _context.Users
//                 .FirstOrDefault(u => u.SessionToken == sessionToken);

//             if (user != null)
//             {
//                 user.SessionToken = null;
//                 _context.SaveChanges();
//             }
//         }
//     }
// }




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

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return null;

            // verify password
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isValid)
                return null;

            // generate session token
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

            email = email.Trim().ToLower();

            // prevent duplicate email
            if (_context.Users.Any(u => u.Email == email))
                return null;

            var user = new User
            {
                Name = name.Trim(),
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