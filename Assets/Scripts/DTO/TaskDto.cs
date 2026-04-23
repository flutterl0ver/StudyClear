using System;

[Serializable]
public class TaskDto
{
    public string Title { get; set; }
    public string Subject { get; set; }
    public DateTime Deadline { get; set; }
}
