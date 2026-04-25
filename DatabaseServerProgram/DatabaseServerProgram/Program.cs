using DatabaseServerProgram.Classes.Services;
using DatabaseServerProgram.Classes.Services.Http.Controllers;
using DatabaseServerProgram.Database;

namespace DatabaseServerProgram;

internal static class Program {
    private static void Main(string[] args) {
        using (ApplicationDatabaseContext context = new()) {
            context.Database.EnsureCreated();
        }

        HttpNetworkService networkService = new();

        networkService.Post<UserController>("register", "Register");
        networkService.Post<UserController>("login", "Login");
        networkService.Post<UserController>("{user}/set-parent", "SetParent");
        networkService.Post<UserController>("{user}/set-teacher", "SetTeacher");
        networkService.Post<TaskController>("create-task", "CreateTask");
        networkService.Post<AiController>("ai/send-message", "SendMessage");
        
        networkService.Get<TaskController>("{user}/tasks", "GetTasks");
        networkService.Get<UserController>("{user}/children", "GetChildren");
        
        networkService.StartListeningAsync().Wait();
    }
}