using System;
using TMPro;
using UnityEngine;

public class CreateTaskWindow : MonoBehaviour {
    [SerializeField]
    private TMP_InputField _taskTitle, _taskSubject, _taskDate;

    public void CreateTask() {
        TaskDto taskData = new() {
            Title = _taskTitle.text,
            Subject = _taskSubject.text,
            Deadline = DateTime.Parse(_taskDate.text)
        };
        
        TasksManager.Instance.CreateTask(taskData);
    }
}