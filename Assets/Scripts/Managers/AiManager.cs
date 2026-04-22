using System;
using System.Text.Json;
using TMPro;
using LLMUnity;
using UnityEngine;

public class AiManager : Singleton<AiManager> {
    [SerializeField]
    private LLMAgent _llmAgent;
    
    [SerializeField]
    private TextMeshProUGUI _resultText;

    private TaskDto _jsonResult;
    
    private void HandleReply(string reply) {
        int jsonPos = reply.IndexOf("```", StringComparison.InvariantCulture);

        if (jsonPos == -1) {
            _resultText.text = reply;
            return;
        }

        if (jsonPos + 7 >= reply.Length) return;
        
        int endJsonPos = reply.IndexOf("```", jsonPos + 7, StringComparison.InvariantCulture);
        
        if(endJsonPos != -1) 
        {
            if (_jsonResult == null) {
                int startPos = jsonPos + 7;
                int endPos = endJsonPos;
                _jsonResult = JsonSerializer.Deserialize<TaskDto>(reply.Substring(startPos, endPos - startPos));
                TasksManager.Instance.CreateTask(_jsonResult);
            }
            _resultText.text = reply.Substring(0, jsonPos) + '\n' + reply.Substring(endJsonPos + 3);
        }
    }

    public void SendMessage(string text) {
        text = $"[{DateTime.Now}] {text}";
        
        _ = _llmAgent.Chat(text, HandleReply);
    }
}