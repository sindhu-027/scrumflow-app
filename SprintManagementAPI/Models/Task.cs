// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace SprintManagementAPI.Models
// {
//     public class TaskModel
//     {
//         public int Id { get; set; }

//         [Required]
//         [MaxLength(200)]
//         public string Title { get; set; } = "";

//         public string? Description { get; set; }

//         [Required]
//         public string Status { get; set; } = "To Do"; // To Do, In Progress, Done

//         [Required]
//         public string Priority { get; set; } = "Medium"; // High, Medium, Low

//         // 🔥 Story Feature
//         public string? StoryName { get; set; }

//         public int StoryPoints { get; set; } = 0;

//         // 🔗 Relations
//         public int? SprintId { get; set; }
//         public int? AssigneeId { get; set; }

//         // Navigation
//         [ForeignKey("SprintId")]
//         public Sprint? Sprint { get; set; }

//         [ForeignKey("AssigneeId")]
//         public User? Assignee { get; set; }
//     }
// }




//----------------------------------------------------------------




using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintManagementAPI.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [MaxLength(50)] // 🔥 prevent invalid long values
        public string Status { get; set; } = "To Do"; // To Do, In Progress, Done

        [Required]
        [MaxLength(20)] // 🔥 prevent invalid long values
        public string Priority { get; set; } = "Medium"; // High, Medium, Low

        // 🔥 Story Feature
        public string? StoryName { get; set; }

        public int StoryPoints { get; set; } = 0;

        // 🔗 Relations
        public int? SprintId { get; set; }
        public int? AssigneeId { get; set; }

        // Navigation
        [ForeignKey("SprintId")]
        public Sprint? Sprint { get; set; }

        [ForeignKey("AssigneeId")]
        public User? Assignee { get; set; }
    }
}