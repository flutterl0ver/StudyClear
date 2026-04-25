using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : ReplacingSingleton<MainScreen> {
    [SerializeField]
    private Image _doneTasksProgress, _lowTimeTasksProgress, _expiredTasksProgress;
    
    [SerializeField]
    private TextMeshProUGUI _progressText, _nameText;

    [SerializeField]
    private StatsSection _statsSection;
    
    [SerializeField]
    private CreateTaskWindow _createTaskWindow;

    [SerializeField]
    private GameObject _tasksMenu;

    protected new void OnEnable() {
        base.OnEnable();

        _nameText.text = $"Добрый день, {UserManager.Instance.User.Name}.";
    }
    
    public void UpdateProgress(int doneTasks, int lowTimeTasks, int expiredTasks, int normalTasks) {
        if (_doneTasksProgress == null) return;
        
        int total = doneTasks + lowTimeTasks + expiredTasks + normalTasks;
        
        if (total == 0) {
            _progressText.text = "Нет заданий";
            return;
        }
        
        _doneTasksProgress.fillAmount = (float)doneTasks / total;
        _expiredTasksProgress.fillAmount = (float)expiredTasks / total;
        _lowTimeTasksProgress.fillAmount = (float)lowTimeTasks / total + _expiredTasksProgress.fillAmount;

        _progressText.text = $"{doneTasks}/{total}";
    }

    public void OpenChildStats(UserDto child) {
        _statsSection.gameObject.SetActive(true);
        
        _statsSection.LoadStats(child);
    }

    public void OpenStudentStats(UserDto student) {
        _tasksMenu.SetActive(true);
        
        _createTaskWindow.SelectedUser = student;
        
        string tasksResponse = HttpService.SendGetRequest($"{student.Username}/tasks");
        List<TaskDto> tasks = JsonSerializer.Deserialize<List<TaskDto>>(tasksResponse);
        
        TasksManager.Instance.LoadTasks(tasks, false);
        _statsSection.LoadStats(student);
    }
}