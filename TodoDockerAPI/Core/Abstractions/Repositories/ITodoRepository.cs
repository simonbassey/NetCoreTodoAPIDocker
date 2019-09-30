using System;
using System.Threading.Tasks;
using TodoDockerAPI.Core.Models.Domain;
using System.Collections.Generic;
namespace TodoDockerAPI.Core.Abstractions.Repositories
{
    public interface ITodoRepository
    {
        Task<TodoItem> AddTodo(TodoItem todo);
        Task<TodoItem> GetTodo(int todoId);
        Task<bool> TodoExist(string title);
        Task<bool> UpdateTodo(int todoId, TodoItem todo);
        Task<bool> RemoveTodo(int todoId);
        Task<IEnumerable<TodoItem>> GetTodos();
        Task<IEnumerable<TodoItem>> GetTodos(Func<TodoItem, bool> filter);
    }
}
