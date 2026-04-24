namespace DatabaseServerProgram.Database.Entities;

public class Task {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subject { get; set; }
    public DateTime Deadline { get; set; }
    public string User { get; set; }

    public Task() { }

    public Task(string title, string subject, DateTime deadline, string user) {
        Title = title;
        Subject = subject;
        Deadline = deadline;
        User = user;
    }
}