using System;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class AiManager : Singleton<AiManager> {
    [SerializeField]
    private PromptsConfig _promptsConfig;

    public void SendMessage(string text, Action<string> handleReply, [CanBeNull] Action onEnd = null) {
        text = $"The user's message: {text}";

        _ = Send(text, _promptsConfig.ChatSystemPrompt, handleReply, onEnd);
    }

    public void GenerateTest(TaskDto task, Action<string> handleReply, [CanBeNull] Action onEnd = null) {
        string prompt = $"Task subject: {task.Subject}\nTask title: {task.Title}";

        _ = Send(prompt, _promptsConfig.GenerateTestSystemPrompt, handleReply, onEnd);
    }

    private async Task Send(string text, string systemPrompt, Action<string> handleReply, [CanBeNull] Action onEnd = null) {
        string responseText = string.Empty;
        systemPrompt += $" Today is {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}.";

        try {
            string response = await HttpService.SendPostRequestAsync("ai/send-message", new { prompt = text, systemPrompt });

            using (JsonDocument doc = JsonDocument.Parse(response)) {
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