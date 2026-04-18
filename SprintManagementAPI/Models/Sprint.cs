// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;

// namespace SprintManagementAPI.Models
// {
//     public class Sprint
//     {
//         public int Id { get; set; }

//         [Required]
//         [MaxLength(150)]
//         public string Name { get; set; } = "";

//         [Required]
//         public DateTime StartDate { get; set; }

//         [Required]
//         public DateTime EndDate { get; set; }

//         [Required]
//         public string Description { get; set; } = "";

//         // ✅ NEW FIELD (IMPORTANT)
//         [Required]
//         public string Status { get; set; } = "Planned"; 
//         // Planned | Active | Completed

//         // Optional navigation property
//         public List<TaskModel>? Tasks { get; set; }
//     }
// }

//------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SprintManagementAPI.Models
{
    public class Sprint
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        // ✅ Status: Planned | Active | Completed
        [Required]
        [MaxLength(20)] // 🔥 prevents large invalid values
        public string Status { get; set; } = "Planned";

        // Navigation
        public List<TaskModel> Tasks { get; set; } = new();
    }
}