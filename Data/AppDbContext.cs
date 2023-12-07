using System.Collections.Generic;
using System.Reflection.Emit;
using TaskManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TaskManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add configurations if needed
        }
    }
}
