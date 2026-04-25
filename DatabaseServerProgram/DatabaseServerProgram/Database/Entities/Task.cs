namespace DatabaseServerProgram.Database.Entities;

public class Task {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subject { get; set; }
    public DateTime Deadline { get; set; }
    public string User { get; set; }
    public List<Subtask> Subtasks { get; set; } = new();
    public short Score { get; set; }

    public Task() { }

    public Task(string title, string subject, DateTime deadline, string user, List<Subtask> subtasks, short score = -1) {
        Title = title;
        Subject = subject;
        Deadline = deadline;
        User = user;
        Subtasks = subtasks;
        Score = score;
    }
}

public class Subtask {
    public int Id { get; set; }
    public string Title { get; set; }
}