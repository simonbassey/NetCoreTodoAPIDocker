using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoDockerAPI.Core.Abstractions.Repositories;
using TodoDockerAPI.Core.Abstractions.Services;
using TodoDockerAPI.Core.Models.Domain;

namespace TodoDockerAPI.Services
{
    public class TodoService : ITodoService
    {
        readonly ITodoRepository _todoRepository;
        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoItem> CreateTodo(TodoItem todo)
        {
            try
            {
                var todoExist = await _todoRepository.TodoExist(todo.Title);
                if (todoExist)
                    return null;
                return await _todoRepository.AddTodo(todo);
            }
            catch (Exception exception)
            {
                // LogException with global Logger
                throw exception;
            }
        }

        public async Task<bool> DeleteTodo(int todoId)
        {
            try
            {
                return await _todoRepository.RemoveTodo(todoId);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<TodoItem> GetTodo(int todoId)
        {
            try
            {
                return await _todoRepository.GetTodo(todoId);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TodoItem>> GetTodos()
        {
            try
            {
                return await _todoRepository.GetTodos();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TodoItem>> GetTodos(Func<TodoItem, bool> filter)
        {
            try
            {
                return await _todoRepository.GetTodos(filter);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<bool> TodoExist(string title) => await _todoRepository.TodoExist(title);
        public async Task<bool> UpdateTodoDetails(int todoId, string title, string description, bool completed = false)
        {
            try
            {
                var todo = await GetTodo(todoId);
                if (todo == null)
                    return false;
                todo.Title = title;
                todo.Description = description;
                todo.Completed = completed ? completed : todo.Completed;

                return await _todoRepository.UpdateTodo(todoId, todo);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTodoStatus(int todoId, bool status)
        {
            try
            {
                var todo = await GetTodo(todoId);
                if (todo == null)
                    return false;
                todo.Completed = status;
                return await _todoRepository.UpdateTodo(todoId, todo);
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
