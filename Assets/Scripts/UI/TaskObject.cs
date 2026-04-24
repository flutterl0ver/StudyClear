using System.Globalization;
using UnityEngine;
using TMPro;

public class TaskObject : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _titleText, _subjectText, _dateText;

    [SerializeField]
    private Color _expiredDateColor, _lowTimeColor;

    public TaskDto TaskData { get; set; }

    public void Init(TaskDto taskData) {
        TaskData = taskData;

        _titleText.text = taskData.Title;
        _subjectText.text = taskData.Subject;
        _dateText.text = taskData.Deadline.ToString(CultureInfo.InvariantCulture);

        if (taskData.IsExpired) {
            _dateText.color = _expiredDateColor;
        } else if (taskData.IsLowTime) {
            _dateText.color = _lowTimeColor;
        }
    }

    public void GenerateTest() {
        TestsManager.Instance.GenerateTest(TaskData);
    }
}