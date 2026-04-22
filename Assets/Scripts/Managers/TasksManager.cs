using UnityEngine;
using UnityEngine.UI;

public class TasksManager : Singleton<TasksManager> {
    [SerializeField]
    private TaskObject _taskPrefab;

    [SerializeField]
    private Transform _tasksContainer;
    
    [SerializeField]
    private RectTransform _rebuildingLayout;

    public void CreateTask(TaskDto taskData) {
        TaskObject newTask = Instantiate(_taskPrefab, _tasksContainer);
        
        newTask.Init(taskData);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_tasksContainer as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rebuildingLayout);
    }
}
