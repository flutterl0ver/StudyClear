using System;

[Serializable]
public class TaskDto {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subject { get; set; }
    public DateTime Deadline { get; set; }
    public string User { get; set; }
    
    public bool IsExpired => Deadline < DateTime.Now;
    public bool IsLowTime => Deadline < DateTime.Now.AddDays(1);
}