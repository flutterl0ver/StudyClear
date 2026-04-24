using System;
using System.Collections;
using System.Globalization;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBubble : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _firstText, _secondText, _taskSubject, _taskName, _taskDeadline, _taskSubtasks;

    [SerializeField]
    private GameObject _createdTask;

    private IEnumerator SkipFrameCoroutine(Action callback) {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        callback();
    }
    
    public void Init(string firstText, string secondText = "", [CanBeNull] TaskDto createdTask = null) {
        _firstText.text = firstText;
        
        if(secondText != string.Empty) _secondText.text = secondText;

        if (createdTask != null) {
            _createdTask.SetActive(true);

            _taskSubject.text = createdTask.Subject;
            _taskName.text = createdTask.Title;
            _taskDeadline.text = createdTask.Deadline.ToString(CultureInfo.InvariantCulture);
            if (createdTask.Subtasks.Count > 0) {
                string subtasks = createdTask.Subtasks.Count switch {
                    1 => "подзадача",
                    < 5 => "подзадачи",
                    _ => "подзадач"
                };
                
                _taskSubtasks.text = $"(+ {createdTask.Subtasks.Count} {subtasks})";
            }
            else _taskSubtasks.gameObject.SetActive(false);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);

        if (ChatManager.Instance.IsChatOpened) {
            StartCoroutine(SkipFrameCoroutine(ChatManager.Instance.MoveChatDown));
        }
        else ChatManager.Instance.ShowNotificationIcon();
    }
}
