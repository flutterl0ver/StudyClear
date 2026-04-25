using System.Text.Json;
using DatabaseServerProgram.Classes.Providers;
using DatabaseServerProgram.Database.Entities;
using DatabaseServerProgram.DTO;
using DatabaseServerProgram.Interfaces;

namespace DatabaseServerProgram.Classes.Services.Http.Controllers;

public class TaskController : IHttpController {
    public object HandleRequest(HttpRequest request) {
        throw new NotImplementedException();
    }

    public object GetTasks(HttpRequest request) {
        string username = request.UrlVariables["user"];
        
        return TasksProvider.GetTasks(username);
    }

    public object CreateTask(HttpRequest request) {
        string title = request.RequestBody["Title"];
        string subject = request.RequestBody["Subject"];
        DateTime deadline = DateTime.Parse(request.RequestBody["Deadline"]);
        string username = request.RequestBody["User"];
        List<Subtask> subtasks = JsonSerializer.Deserialize<List<Subtask>>(request.RequestBody["Subtasks"]) ?? new();

        Database.Entities.Task task = new(title, subject, deadline, username, subtasks);
        
        TasksProvider.AddTask(task);

        return task;
    }
}