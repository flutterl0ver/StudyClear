using DatabaseServerProgram.Database;

namespace DatabaseServerProgram.Classes.Providers;

public static class TasksProvider {
    public static List<Database.Entities.Task> GetTasks(string username) {
        using (ApplicationDatabaseContext context = new()) {
            return context.Tasks
                .Where(t => t.User == username)
                .OrderBy(t => t.Deadline)
                .ToList();
        }
    }

    public static void AddTask(Database.Entities.Task task) {
        using (ApplicationDatabaseContext context = new()) {
            context.Tasks.Add(task);
            context.SaveChanges();
        }
    }
}