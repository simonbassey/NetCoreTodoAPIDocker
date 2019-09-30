using System;
using TodoDockerAPI.Data.Core.Repositories;
using TodoDockerAPI.Core.Models.Domain;
using TodoDockerAPI.Core.Abstractions.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TodoDockerAPI.Data.Repositories
{
    public class TodoRepository : BaseRepository<TodoItem>, ITodoRepository
    {
        public TodoRepository()
        {
        }

        public async Task<TodoItem> AddTodo(TodoItem todo)
        {
            try
            {
                var saveResult = await AddItemAsync(todo);
                return saveResult;
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
                var todo = await GetItemAsync(todoId);
                return todo;
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
                var todoList = await GetItemsAsync();
                return todoList;
            }
            catch (Exception excption)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TodoItem>> GetTodos(Func<TodoItem, bool> filter)
        {
            try
            {
                var todos = await FilterAsync(filter);
                return todos;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveTodo(int todoId)
        {
            try
            {
                var todo = await GetItemAsync(todoId);
                if (todo == null)
                    return false;
                return await RemoveItemAsync(todoId);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<bool> TodoExist(string title)
        {
            try
            {
                return (await GetTodos(t => t.Title.ToLower() == title.ToLower())).FirstOrDefault() != null;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTodo(int todoId, TodoItem todo)
        {
            try
            {
                var targetTodo = await GetTodo(todoId);
                if (targetTodo == null)
                    return false;
                targetTodo.Title = todo.Title;
                targetTodo.Description = todo.Description;
                targetTodo.Completed = todo.Completed;
                targetTodo.LastUpdated = DateTime.Now;
                var updateResult = await UpdateItemAsync(targetTodo, todoId);
                return updateResult != null;
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
