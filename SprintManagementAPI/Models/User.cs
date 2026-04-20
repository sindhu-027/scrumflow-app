using System.ComponentModel.DataAnnotations;

namespace SprintManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // 🔥 Stored as HASH (BCrypt)
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Roles: Admin, Scrum Master, Developer
        [Required]
        [MaxLength(50)] // 🔥 prevents invalid long roles
        public string Role { get; set; } = "Developer";

        // 🔥 Cookie-based session
        public string? SessionToken { get; set; }

        // ✅ Profile Avatar (optional)
        [MaxLength(500)] // 🔥 prevent huge values
        public string? Avatar { get; set; }
    }
}