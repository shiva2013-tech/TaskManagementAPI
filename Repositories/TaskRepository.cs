using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories
{
    public class TaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }

        public TaskModel GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public void InsertTask(TaskModel task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            task.IsActive = true;
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(TaskModel task)
        {
            //task.UpdatedAt = DateTime.UtcNow;

            //_context.Tasks.Update(task);
            //_context.SaveChanges();
            var existingTask = _context.Tasks.Find(task.Id);

            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.LastDate = task.LastDate;
                existingTask.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        public void SoftDeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                task.IsActive = false;
                task.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }
    }
}
