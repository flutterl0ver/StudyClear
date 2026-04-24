using System;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class AiManager : Singleton<AiManager> {
    [SerializeField]
    [TextArea(10, 10)]
    private string _systemPrompt;

    public void SendMessage(string text, Action<string> handleReply, [CanBeNull] Action onEnd = null) {
        text = $"The user's message:\n[{DateTime.Now.DayOfWeek} {DateTime.Now}] {text}";

        _ = Send(text, handleReply, onEnd);
    }

    private async Task Send(string text, Action<string> handleReply, [CanBeNull] Action onEnd = null) {
        string responseText = string.Empty;

        try {
            string response = await HttpService.SendPostRequestAsync("ai/send-message", new { prompt = text, systemPrompt = _systemPrompt });

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