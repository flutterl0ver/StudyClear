using System;
using System.Collections;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : Singleton<ChatManager> {
    [SerializeField]
    private TMP_InputField _messageField;

    [SerializeField]
    private Transform _messagesContainer;

    [SerializeField]
    private MessageBubble _botMessagePrefab, _userMessagePrefab;

    [SerializeField]
    private GameObject _microButton, _sendButton, _menuNotificationIcon, _loadingIconPrefab;
    
    [SerializeField]
    private ScrollRect _messagesScrollRect;

    public bool IsChatOpened {
        get => _isChatOpened;
        set {
            _isChatOpened = value;
            if (value) OnOpenChat();
        }
    }
    
    private TaskDto _jsonResult;
    private string _botReply;
    private bool _isChatOpened;
    private GameObject _loadingIcon;

    public void SendMessage() {
        MessageBubble message = Instantiate(_userMessagePrefab, _messagesContainer);
        message.Init(_messageField.text);
        
        AiManager.Instance.SendMessage(_messageField.text, HandleReply, EndReply);

        if (_loadingIcon == null) {
            _loadingIcon = Instantiate(_loadingIconPrefab, _messagesContainer);
        }

        _messageField.text = string.Empty;
    }

    public void InputText(string text) {
        bool empty = text == string.Empty;
        
        _microButton.SetActive(empty);
        _sendButton.SetActive(!empty);
    }

    public void MoveChatDown() {
        _messagesScrollRect.verticalNormalizedPosition = 0;
    }

    public void ShowNotificationIcon() {
        _menuNotificationIcon.SetActive(true);
    }
    
    private void HandleReply(string reply) {
        _botReply = reply;
    }

    private IEnumerator RebuildMessagesCoroutine() {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_messagesContainer as RectTransform);
    }
    
    private void OnOpenChat() {
        StartCoroutine(RebuildMessagesCoroutine());
    }

    private void EndReply() {
        Destroy(_loadingIcon);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_messagesContainer as RectTransform);
        
        int jsonPos = _botReply.IndexOf("```", StringComparison.InvariantCulture);
        MessageBubble message = Instantiate(_botMessagePrefab, _messagesContainer);

        try {
            if (jsonPos == -1) {
                message.Init(_botReply);
                return;
            }

            if (jsonPos + 7 >= _botReply.Length) return;

            int endJsonPos = _botReply.IndexOf("```", jsonPos + 7, StringComparison.InvariantCulture);

            if (endJsonPos != -1) {
                if (_jsonResult == null) {
                    int startPos = jsonPos + 7;
                    int endPos = endJsonPos;
                    _jsonResult = JsonSerializer.Deserialize<TaskDto>(_botReply.Substring(startPos, endPos - startPos));
                    TasksManager.Instance.CreateTask(_jsonResult);
                }

                message.Init(_botReply.Substring(0, Math.Max(0, jsonPos - 1)), _botReply.Substring(endJsonPos + 3), _jsonResult);
            }
        } catch (Exception e) {
            Debug.LogException(e);
            message.Init("Извините, что-то пошло не так.");
        }
        
        _jsonResult = null;
        _botReply = string.Empty;
    }
}