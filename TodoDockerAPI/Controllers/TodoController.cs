using System;
using Microsoft.AspNetCore.Mvc;
using TodoDockerAPI.Core.Abstractions.Services;
using System.Threading.Tasks;
using System.Net;
using TodoDockerAPI.Core.Models.Domain;
using TodoDockerAPI.Core.Models.RequestObjects;
namespace TodoDockerAPI.Controllers
{
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var todoList = await _todoService.GetTodos();
                return Ok(todoList);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpGet("{todoId}")]
        public async Task<IActionResult> GetTodo(int todoId)
        {
            try
            {
                var todo = await _todoService.GetTodo(todoId);
                if (todo == null)
                    return BadRequest($"No Todo Item was found with provided Id {todoId}");
                return Ok(todo);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTodo([FromBody] TodoCreateRequest request)
        {
            try
            {
                var error = ManuallyValidateNewTodoPayload(request);
                if (!string.IsNullOrEmpty(error))
                    return BadRequest(error);
                var todo = new TodoItem(request.Title, request.Description);
                var todoExist = await _todoService.TodoExist(todo.Title);
                if (todoExist)
                    return BadRequest($"A similar todo with title {todo.Title} already exist");
                var createResult = await _todoService.CreateTodo(todo);
                if (createResult == null)
                    return StatusCode((int)HttpStatusCode.NotFound);
                return Created(new Uri($"/{createResult.Id}"), createResult);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody]TodoItem todo)
        {
            try
            {
                return Ok(await _todoService.UpdateTodoDetails(id, todo.Title, todo.Description, todo.Completed));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                return Ok(await _todoService.DeleteTodo(id));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        private string ManuallyValidateNewTodoPayload(TodoCreateRequest item)
        {
            if (item == null)
                return "Request payload is empty";
            if (string.IsNullOrEmpty(item.Title))
                return "ACtivity title is required";
            return string.Empty;
        }
    }
}
