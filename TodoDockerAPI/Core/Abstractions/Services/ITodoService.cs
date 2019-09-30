using System;
using System.Threading.Tasks;
using TodoDockerAPI.Core.Models.Domain;
using System.Collections.Generic;
using System.Collections;
namespace TodoDockerAPI.Core.Abstractions.Services
{
    public interface ITodoService
    {
        Task<TodoItem> CreateTodo(TodoItem todo);
        Task<TodoItem> GetTodo(int todoId);
        Task<bool> DeleteTodo(int todoId);
        Task<bool> UpdateTodoStatus(int todoId, bool status);
        Task<bool> UpdateTodoDetails(int todoId, string title, string description, bool completed = false);
        Task<IEnumerable<TodoItem>> GetTodos();
        Task<IEnumerable<TodoItem>> GetTodos(Func<TodoItem, bool> filter);
        Task<bool> TodoExist(string title);
    }
}
