// using System;

// namespace SprintManagementAPI.DTOS
// {
//     // ================= AUTH =================
//     public class LoginDto
//     {
//         public string Email { get; set; } = string.Empty;
//         public string PasswordHash { get; set; } = string.Empty;
//     }

//     public class RegisterDto
//     {
//         public string Name { get; set; } = string.Empty;
//         public string Email { get; set; } = string.Empty;
//         public string PasswordHash { get; set; } = string.Empty;
//     }

//     // ================= USER =================
//     public class RoleDto
//     {
//         public string Role { get; set; } = string.Empty;
//     }

//     // ================= TASK =================
//     public class TaskDto
//     {
//         public string Title { get; set; } = string.Empty;
//         public string? Description { get; set; }

//         public string Status { get; set; } = "To Do";
//         public string Priority { get; set; } = "Medium";

//         public string? StoryName { get; set; }
//         public int StoryPoints { get; set; } = 0;

//         public int? SprintId { get; set; }
//         public int? AssigneeId { get; set; }
//     }

//     // ================= SPRINT =================
//     public class SprintDto
//     {
//         // 🔥 FIXED (no warnings now)
//         public string Name { get; set; } = string.Empty;
//         public string Description { get; set; } = string.Empty;

//         public DateTime StartDate { get; set; }
//         public DateTime EndDate { get; set; }

//         public string Status { get; set; } = "Planned";
//     }
// }




using System;

namespace SprintManagementAPI.DTOS
{
    // ================= AUTH =================
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;

        // FIX: was PasswordHash ❌
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // FIX: was PasswordHash ❌
        public string Password { get; set; } = string.Empty;
    }

    // ================= USER =================
    public class RoleDto
    {
        public string Role { get; set; } = string.Empty;
    }

    // ================= TASK =================
    public class TaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string Status { get; set; } = "To Do";
        public string Priority { get; set; } = "Medium";

        public string? StoryName { get; set; }
        public int StoryPoints { get; set; } = 0;

        public int? SprintId { get; set; }
        public int? AssigneeId { get; set; }
    }

    // ================= SPRINT =================
    public class SprintDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Planned";
    }
}