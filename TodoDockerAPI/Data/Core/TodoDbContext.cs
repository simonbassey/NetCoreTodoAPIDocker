using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoDockerAPI.Core.Helpers;
using TodoDockerAPI.Core.Models.Domain;

namespace TodoDockerAPI.Data.Core
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext()
        {
        }

        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ServiceResolver.Resolve<IConfiguration>().GetConnectionString("TodoAppDb");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(connectionString);
            }
        }


        public virtual DbSet<TodoItem> Todos { get; set; }
    }
}
