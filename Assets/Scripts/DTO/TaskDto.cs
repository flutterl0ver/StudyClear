using System;
using System.Collections.Generic;

[Serializable]
public class TaskDto {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subject { get; set; }
    public DateTime Deadline { get; set; }
    public string User { get; set; }
    public List<SubtaskDto> Subtasks { get; set; }
    
    public bool IsExpired => Deadline < DateTime.Now;
    public bool IsLowTime => Deadline < DateTime.Now.AddDays(1);
}

[Serializable]
public class SubtaskDto {
    public string Title { get; set; }
}