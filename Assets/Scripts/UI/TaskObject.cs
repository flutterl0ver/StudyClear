using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskObject : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _titleText, _subjectText, _dateText, _scoreText;

    [SerializeField]
    private Color _expiredDateColor, _lowTimeColor;
    
    [SerializeField]
    private RectTransform _subtasksContainer, _subtaskPrefab;

    [SerializeField]
    private GameObject _scoreObject, _testButton;

    [SerializeField]
    private Image _scoreImage;
    
    [SerializeField]
    private Gradient _scoreGradient;

    public TaskDto TaskData { get; set; }

    public void Init(TaskDto taskData, bool generateTest = true) {
        TaskData = taskData;

        if(!generateTest) Destroy(_testButton);
        
        _titleText.text = taskData.Title;
        _subjectText.text = taskData.Subject;
        _dateText.text = taskData.Deadline.ToString(CultureInfo.InvariantCulture);

        if (taskData.IsExpired) {
            _dateText.color = _expiredDateColor;
        } else if (taskData.IsLowTime) {
            _dateText.color = _lowTimeColor;
        }

        if (taskData.Subtasks.Count > 0) {
            foreach (SubtaskDto subtask in taskData.Subtasks) {
                RectTransform newSubtask = Instantiate(_subtaskPrefab, _subtasksContainer);
                newSubtask.GetComponentInChildren<TextMeshProUGUI>().text = subtask.Title;
            }
        }
        else _subtasksContainer.gameObject.SetActive(false);

        if (taskData.Score != -1) {
            _scoreObject.SetActive(true);
            _scoreImage.color = _scoreGradient.Evaluate(taskData.Score / 10f);
            _scoreText.text = $"{taskData.Score}/10";
        }
    }

    public void GenerateTest() {
        TestsManager.Instance.GenerateTest(TaskData);
    }
}