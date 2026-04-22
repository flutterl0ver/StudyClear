using System;
using JetBrains.Annotations;
using LLMUnity;
using UnityEngine;

public class AiManager : Singleton<AiManager> {
    [SerializeField]
    private LLMAgent _llmAgent;

    public void SendMessage(string text, Action<string> handleReply, [CanBeNull] Action onEnd = null) {
        text = $"[{DateTime.Now}] {text}";
        
        _ = _llmAgent.Chat(text, handleReply, onEnd);
    }
}