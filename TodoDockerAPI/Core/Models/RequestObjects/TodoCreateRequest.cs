using System;
using System.ComponentModel.DataAnnotations;

namespace TodoDockerAPI.Core.Models.RequestObjects
{
    public class TodoCreateRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please add a little description for this activity")]
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
