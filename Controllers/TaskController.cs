using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;

        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var tasks = _taskRepository.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _taskRepository.GetTaskById(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public IActionResult InsertTask([FromBody] TaskModel task)
        {
            _taskRepository.InsertTask(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskModel task)
        {
            var existingTask = _taskRepository.GetTaskById(id);
            if (existingTask == null)
                return NotFound();

            task.Id = id;
            _taskRepository.UpdateTask(task);

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult SoftDeleteTask(int id)
        {
            var existingTask = _taskRepository.GetTaskById(id);
            if (existingTask == null)
                return NotFound();

            _taskRepository.SoftDeleteTask(id);

            return NoContent();
        }
    }
}
