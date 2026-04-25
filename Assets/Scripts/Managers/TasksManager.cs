using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;

public class TasksManager : Singleton<TasksManager> {
    [SerializeField]
    private TaskObject _taskPrefab;

    [SerializeField]
    private Transform _activeTasksContainer, _expiredTasksContainer, _checkingTasksContainer;

    [SerializeField]
    private RectTransform _rebuildingLayout;

    [SerializeField]
    private GameObject _notificationIcon, _noExpiredImage;

    [SerializeField]
    private TextMeshProUGUI _activeText, _lowTimeText, _expiredText;

    private int _activeCount, _lowTimeCount, _expiredCount;

    public void LoadTasks(List<TaskDto> tasks, bool generateTests = true) {
        ClearTasks();
        foreach (TaskDto task in tasks) {
            CreateTask(task, false, generateTests);
        }
    }

    public void ClearTasks() {
        foreach (RectTransform child in _activeTasksContainer) {
            Destroy(child.gameObject);
        }
        
        foreach (RectTransform child in _checkingTasksContainer) {
            Destroy(child.gameObject);
        }
        
        foreach (RectTransform child in _expiredTasksContainer) {
            if (child == _noExpiredImage.transform) continue;
            
            Destroy(child.gameObject);
        }
    }

    public void CheckExpired() {
        _noExpiredImage.SetActive(_expiredCount == 0);
    }

    public void CreateTask(TaskDto taskData, bool save = true, bool generateTest = true, UserDto user = null) {
        taskData.User = (user ?? UserManager.Instance.User).Username;
        if (save) {
            taskData = JsonSerializer.Deserialize<TaskDto>(HttpService.SendPostRequest("create-task", taskData));
            if(_notificationIcon != null) _notificationIcon.SetActive(true);
        }

        Transform taskContainer = _activeTasksContainer;
        if (taskData.IsExpired) {
            _expiredText.text = (++_expiredCount).ToString();

            taskContainer = _expiredTasksContainer;
        } else if (taskData.IsLowTime) {
            _lowTimeText.text = (++_lowTimeCount).ToString();
        } else {
            _activeText.text = (++_activeCount).ToString();
        }

        MainScreen.Instance.UpdateProgress(0, _lowTimeCount, _expiredCount, _activeCount);

        TaskObject newTask = Instantiate(_taskPrefab, taskContainer);

        newTask.Init(taskData, generateTest);
    }
}