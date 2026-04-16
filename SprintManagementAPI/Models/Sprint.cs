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
        public string Name { get; set; } = "";

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Description { get; set; } = "";

        // ✅ NEW FIELD (IMPORTANT)
        [Required]
        public string Status { get; set; } = "Planned"; 
        // Planned | Active | Completed

        // Optional navigation property
        public List<TaskModel>? Tasks { get; set; }
    }
}