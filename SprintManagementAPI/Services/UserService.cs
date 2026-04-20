using SprintManagementAPI.Models;
using SprintManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using SprintManagementAPI.DTOS;

namespace SprintManagementAPI.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // ================= USERS =================

        public List<User> GetAllUsers()
        {
            return _context.Users.AsNoTracking().ToList();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool UpdateUser(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            // ✅ SAFE UPDATE
            user.Name = string.IsNullOrWhiteSpace(updatedUser.Name) ? user.Name : updatedUser.Name;
            user.Email = string.IsNullOrWhiteSpace(updatedUser.Email) ? user.Email : updatedUser.Email;

            _context.SaveChanges();
            return true;
        }

        public bool AssignRole(int id, string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return false;

            var user = _context.Users.Find(id);
            if (user == null) return false;

            user.Role = role.Trim();
            _context.SaveChanges();
            return true;
        }

        // ================= TASKS =================

        public TaskModel CreateTask(TaskModel task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public List<TaskModel> GetAllTasks()
        {
            return _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.Sprint)
                .AsNoTracking()
                .ToList();
        }

        public TaskModel? GetTask(int id)
        {
            return _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.Sprint)
                .FirstOrDefault(t => t.Id == id);
        }

        public bool UpdateTask(int id, TaskModel updatedTask)
        {
            var task = _context.Tasks.Find(id);
            if (task == null || updatedTask == null) return false;

            task.Title = updatedTask.Title?.Trim() ?? task.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status ?? task.Status;
            task.Priority = updatedTask.Priority ?? task.Priority;
            task.StoryName = updatedTask.StoryName;
            task.StoryPoints = updatedTask.StoryPoints;
            task.SprintId = updatedTask.SprintId;
            task.AssigneeId = updatedTask.AssigneeId;

            _context.SaveChanges();
            return true;
        }

        public List<TaskModel> GetMyTasks(int userId)
        {
            return _context.Tasks
                .Where(t => t.AssigneeId == userId)
                .Include(t => t.Sprint)
                .AsNoTracking()
                .ToList();
        }

        public List<TaskModel> GetTasksBySprint(int sprintId)
        {
            return _context.Tasks
                .Where(t => t.SprintId == sprintId)
                .Include(t => t.Assignee)
                .AsNoTracking()
                .ToList();
        }

        // ================= SPRINTS =================

        public List<Sprint> GetAllSprints()
        {
            return _context.Sprints
                .Include(s => s.Tasks)
                .AsNoTracking()
                .ToList();
        }

        public Sprint CreateSprint(Sprint sprint)
        {
            _context.Sprints.Add(sprint);
            _context.SaveChanges();
            return sprint;
        }

        public Sprint? GetSprintById(int id)
        {
            return _context.Sprints
                .Include(s => s.Tasks)
                .FirstOrDefault(s => s.Id == id);
        }

        public bool UpdateSprint(int id, SprintDto dto)
        {
            var sprint = _context.Sprints.Find(id);
            if (sprint == null) return false;

            sprint.Name = dto.Name;
            sprint.StartDate = dto.StartDate;
            sprint.EndDate = dto.EndDate;
            sprint.Description = dto.Description;
            sprint.Status = dto.Status;

            _context.SaveChanges();
            return true;
        }

        public bool StartSprint(int id)
        {
            var sprint = _context.Sprints.Find(id);
            if (sprint == null) return false;

            sprint.Status = "Active";
            _context.SaveChanges();
            return true;
        }

        public bool EndSprint(int id)
        {
            var sprint = _context.Sprints.Find(id);
            if (sprint == null) return false;

            sprint.Status = "Completed";
            _context.SaveChanges();
            return true;
        }

        // ✅ USED BY FE DELETE API
        public bool DeleteSprint(int id)
        {
            var sprint = _context.Sprints
                .Include(s => s.Tasks)
                .FirstOrDefault(s => s.Id == id);

            if (sprint == null) return false;

            foreach (var task in sprint.Tasks ?? new List<TaskModel>())
            {
                task.SprintId = null;
            }

            _context.Sprints.Remove(sprint);
            _context.SaveChanges();
            return true;
        }
    }
}