using UnityEngine;

[CreateAssetMenu(fileName = "PromptsConfig", menuName = "Scriptable Objects/PromptsConfig")]
public class PromptsConfig : ScriptableObject {
    [TextArea(10, 10)]
    public string ChatSystemPrompt;

    [TextArea(10, 10)]
    public string GenerateTestSystemPrompt;
}