using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBubble : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _firstText, _secondText, _taskName, _taskDeadline;

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

            _taskName.text = createdTask.Title;
            _taskDeadline.text = createdTask.Deadline.ToString();
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);

        if (ChatManager.Instance.IsChatOpened) {
            StartCoroutine(SkipFrameCoroutine(ChatManager.Instance.MoveChatDown));
        }
        else ChatManager.Instance.ShowNotificationIcon();
    }
}
