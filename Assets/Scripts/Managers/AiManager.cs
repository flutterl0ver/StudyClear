using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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

    private void Send(string text, Action<string> handleReply, [CanBeNull] Action onEnd = null) {
        string responseText = string.Empty;

        try {
            HttpClient client = new();

            HttpResponseMessage response = client.PostAsync("http://localhost:11434/api/generate",
                new StringContent(JsonSerializer.Serialize(new { model = "gemma3:4b", prompt = text, stream = false }))).Result;

            string reply = response.Content.ReadAsStringAsync().Result;

            using (JsonDocument doc = JsonDocument.Parse(reply)) {
                responseText = doc.RootElement.GetProperty("response").ToString();
            }
        } catch (Exception e) {
            Debug.LogException(e);
            responseText = "Извините, что-то пошло не так.";
        }
        
        handleReply.Invoke(responseText);
        onEnd?.Invoke();
    }
}