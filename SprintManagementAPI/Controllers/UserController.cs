using Microsoft.AspNetCore.Mvc;
using SprintManagementAPI.Services;
using SprintManagementAPI.Models;
using SprintManagementAPI.DTOS;
using SprintManagementAPI.Utils;

namespace SprintManagementAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public UserController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // ================= USERS =================

        // GET: api/users
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        // ✅ FIX: restrict id to int
        // GET: api/users/1
        [HttpGet("users/{id:int}")]
        public IActionResult GetProfile(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        // ✅ FIX: restrict id to int
        // PUT: api/users/1
        [HttpPut("users/{id:int}")]
        public IActionResult UpdateProfile(int id, [FromBody] User updatedUser)
        {
            var result = _userService.UpdateUser(id, updatedUser);

            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "Profile updated successfully" });
        }

        // PUT: api/users/assign-role/1
        [HttpPut("users/assign-role/{id:int}")]
        public IActionResult AssignRole(int id, [FromBody] RoleDto dto)
        {
            var result = _userService.AssignRole(id, dto.Role);

            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "Role updated successfully" });
        }

        // ================= TASKS =================

        // POST: api/tasks
        [HttpPost("tasks")]
        public IActionResult CreateTask([FromBody] TaskDto dto)
        {
            var task = new TaskModel
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                StoryName = dto.StoryName,
                StoryPoints = dto.StoryPoints,
                SprintId = dto.SprintId,
                AssigneeId = dto.AssigneeId
            };

            return Ok(_userService.CreateTask(task));
        }

        // GET: api/tasks
        [HttpGet("tasks")]
        public IActionResult GetAllTasks()
        {
            return Ok(_userService.GetAllTasks());
        }

        // ✅ FIX: restrict id to int
        // GET: api/tasks/1
        [HttpGet("tasks/{id:int}")]
        public IActionResult GetTask(int id)
        {
            var task = _userService.GetTask(id);

            if (task == null)
                return NotFound(new { message = "Task not found" });

            return Ok(task);
        }

        // ✅ FIX: restrict id to int
        // PUT: api/tasks/1
        [HttpPut("tasks/{id:int}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskDto dto)
        {
            var updatedTask = new TaskModel
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                StoryName = dto.StoryName,
                StoryPoints = dto.StoryPoints,
                SprintId = dto.SprintId,
                AssigneeId = dto.AssigneeId
            };

            var result = _userService.UpdateTask(id, updatedTask);

            if (!result)
                return NotFound(new { message = "Task not found" });

            return Ok(new { message = "Task updated successfully" });
        }

        // GET: api/tasks/my
        [HttpGet("tasks/my")]
        public IActionResult GetMyTasks()
        {
            var sessionId = CookieUtil.GetCookie(Request, "sessionId");

            if (string.IsNullOrEmpty(sessionId))
                return Unauthorized(new { message = "No active session" });

            var user = _authService.GetCurrentUser(sessionId);

            if (user == null)
                return Unauthorized(new { message = "Session expired" });

            return Ok(_userService.GetMyTasks(user.Id));
        }

        // ================= SPRINTS =================

        // GET: api/sprints
        [HttpGet("sprints")]
        public IActionResult GetSprints()
        {
            return Ok(_userService.GetAllSprints());
        }

        // POST: api/sprints
        [HttpPost("sprints")]
        public IActionResult CreateSprint([FromBody] SprintDto dto)
        {
            var sprint = new Sprint
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Description = dto.Description,
                Status = "Planned"
            };

            return Ok(_userService.CreateSprint(sprint));
        }

        // ✅ FIX: restrict id to int
        // PUT: api/sprints/1
        [HttpPut("sprints/{id:int}")]
        public IActionResult UpdateSprint(int id, [FromBody] SprintDto dto)
        {
            var result = _userService.UpdateSprint(id, dto);

            if (!result)
                return NotFound(new { message = "Sprint not found" });

            return Ok(new { message = "Sprint updated successfully" });
        }

        // PUT: api/sprints/1/start
        [HttpPut("sprints/{id:int}/start")]
        public IActionResult StartSprint(int id)
        {
            var result = _userService.StartSprint(id);

            if (!result)
                return BadRequest(new { message = "Cannot start sprint" });

            return Ok(new { message = "Sprint started" });
        }

        // PUT: api/sprints/1/end
        [HttpPut("sprints/{id:int}/end")]
        public IActionResult EndSprint(int id)
        {
            var result = _userService.EndSprint(id);

            if (!result)
                return BadRequest(new { message = "Cannot end sprint" });

            return Ok(new { message = "Sprint completed" });
        }

        // GET: api/sprints/1/tasks
        [HttpGet("sprints/{id:int}/tasks")]
        public IActionResult GetTasksBySprint(int id)
        {
            return Ok(_userService.GetTasksBySprint(id));
        }
    }
}