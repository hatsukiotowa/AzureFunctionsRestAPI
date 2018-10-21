
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static ServerlessFuncs.Todo;
using System.Collections.Generic;
using System.Linq;

namespace ServerlessFuncs
{
    public static class TodoApi
    {
        static List<Todo> items = new List<Todo>();
        
        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "missions")]HttpRequest req, TraceWriter log)
        {
            log.Info("Creating a new todo list item");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { MissionDescription = input.MissionDescription };
            items.Add(todo);
            return new OkObjectResult(todo);
        }

        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetTodos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "missions")]HttpRequest req, TraceWriter log)
        {
            log.Info("Getting todo list items");
            return new OkObjectResult(items);
        }

        [FunctionName("GetTodoById")]
        public static async Task<IActionResult> GetTodoById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mission/{id}")]HttpRequest req, TraceWriter log, string id)
        {
            var todo = items.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(todo);
        }

        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "mission/{id}")]HttpRequest req, TraceWriter log, string id)
        {
            var todo = items.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);

            todo.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrEmpty(updated.MissionDescription))
            {
                todo.MissionDescription = updated.MissionDescription;
            }
            return new OkObjectResult(todo);
        }

        [FunctionName("DeleteTodo")]
        public static async Task<IActionResult> DeleteTodo([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "mission/{id}")]HttpRequest req, TraceWriter log, string id)
        {
            var todo = items.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            items.Remove(todo);
            return new OkResult();
        }
    }
}
