using System.ComponentModel.DataAnnotations;

namespace SprintManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = "";

        // 🔥 Stored as HASH (BCrypt)
        [Required]
        public string PasswordHash { get; set; } = "";

        // Roles: Admin, Scrum Master, Developer
        [Required]
        public string Role { get; set; } = "Developer";

        // 🔥 Cookie-based session
        public string? SessionToken { get; set; }

        // ✅ NEW: Profile Avatar
        public string Avatar { get; set; } = "";
    }
}