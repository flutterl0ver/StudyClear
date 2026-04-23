using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskObject : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _titleText, _descriptionText, _dateText;
    
    public void Init(TaskDto taskData) {
        _titleText.text = $"Название: {taskData.Title}";
        _descriptionText.text = $"Описание: {taskData.Description}";
        _dateText.text = $"Сделать до: {taskData.Deadline}";
    }
}