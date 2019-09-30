using System;
using System.ComponentModel.DataAnnotations;
namespace TodoDockerAPI.Core.Models.Domain
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public TodoItem() { }

        public TodoItem(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
