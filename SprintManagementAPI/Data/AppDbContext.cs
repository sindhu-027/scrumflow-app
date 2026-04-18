// using Microsoft.EntityFrameworkCore;
// using SprintManagementAPI.Models;

// namespace SprintManagementAPI.Data
// {
//     public class AppDbContext : DbContext
//     {
//         public AppDbContext(DbContextOptions<AppDbContext> options)
//             : base(options)
//         {
//         }

//         // ✅ Tables
//         public DbSet<User> Users { get; set; }
//         public DbSet<TaskModel> Tasks { get; set; }
//         public DbSet<Sprint> Sprints { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

//             // ================= USER ↔ TASK =================
//             modelBuilder.Entity<TaskModel>()
//                 .HasOne(t => t.Assignee)
//                 .WithMany()
//                 .HasForeignKey(t => t.AssigneeId)
//                 .OnDelete(DeleteBehavior.SetNull);

//             // ================= SPRINT ↔ TASK =================
//             modelBuilder.Entity<TaskModel>()
//                 .HasOne(t => t.Sprint)
//                 .WithMany(s => s.Tasks)
//                 .HasForeignKey(t => t.SprintId)
//                 .OnDelete(DeleteBehavior.SetNull);

//             // ================= DEFAULT VALUES =================

//             // ✅ Sprint default status
//             modelBuilder.Entity<Sprint>()
//                 .Property(s => s.Status)
//                 .HasDefaultValue("Planned");

//             // ✅ Task default status
//             modelBuilder.Entity<TaskModel>()
//                 .Property(t => t.Status)
//                 .HasDefaultValue("To Do");

//             // ================= VALIDATIONS =================

//             // Prevent long names
//             modelBuilder.Entity<Sprint>()
//                 .Property(s => s.Name)
//                 .HasMaxLength(150);

//             // Optional: enforce required fields (extra safety)
//             modelBuilder.Entity<Sprint>()
//                 .Property(s => s.Description)
//                 .IsRequired();
//         }
//     }
// }







//-----------------------------------




using Microsoft.EntityFrameworkCore;
using SprintManagementAPI.Models;

namespace SprintManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ✅ Tables
        public DbSet<User> Users { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<Sprint> Sprints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= USER ↔ TASK =================
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);

            // ================= SPRINT ↔ TASK =================
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.Sprint)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.SprintId)
                .OnDelete(DeleteBehavior.SetNull);

            // ================= DEFAULT VALUES =================

            modelBuilder.Entity<Sprint>()
                .Property(s => s.Status)
                .HasDefaultValue("Planned");

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.Status)
                .HasDefaultValue("To Do");

            // ================= VALIDATIONS =================

            modelBuilder.Entity<Sprint>()
                .Property(s => s.Name)
                .HasMaxLength(150);

            modelBuilder.Entity<Sprint>()
                .Property(s => s.Description)
                .IsRequired();

            // ================= 🔥 IMPORTANT FIXES =================

            // ✅ Email should be unique (prevents duplicate users)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ✅ PostgreSQL string column safety (optional but good)
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(200);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasMaxLength(150);
        }
    }
}