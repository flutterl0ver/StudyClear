using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : Singleton<MainScreen> {
    [SerializeField]
    private Image _doneTasksProgress, _lowTimeTasksProgress, _expiredTasksProgress;
    
    [SerializeField]
    private TextMeshProUGUI _progressText;
    
    public void UpdateProgress(int doneTasks, int lowTimeTasks, int expiredTasks, int normalTasks) {
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
}