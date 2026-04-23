using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskObject : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _titleText, _subjectText, _dateText;
    
    public void Init(TaskDto taskData) {
        _titleText.text = taskData.Title;
        _subjectText.text = taskData.Subject;
        _dateText.text = taskData.Deadline.ToString(CultureInfo.InvariantCulture);
    }
}