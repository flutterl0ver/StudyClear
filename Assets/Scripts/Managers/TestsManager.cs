using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestsManager : Singleton<TestsManager> {
    [SerializeField]
    private TextMeshProUGUI _testItemPrefab;

    [SerializeField]
    private Transform _testItemsParent;

    [SerializeField]
    private GameObject _loadingIconPrefab, _testsScreen;

    private string _testText;
    private GameObject _loadingIcon;

    public void GenerateTest(TaskDto task) {
        foreach (Transform item in _testItemsParent) {
            Destroy(item.gameObject);
        }

        _testsScreen.SetActive(true);
        _loadingIcon = Instantiate(_loadingIconPrefab, _testItemsParent);

        AiManager.Instance.GenerateTest(task, text => _testText = text, TestGenerated);
    }

    private void TestGenerated() {
        Destroy(_loadingIcon);
        
        string testText = _testText.Substring(7, _testText.Length - 10);

        Dictionary<string, JsonElement> dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(testText);
        List<TestQuestionDto> questions = JsonSerializer.Deserialize<List<TestQuestionDto>>(dict["Questions"].GetRawText());

        foreach (TestQuestionDto question in questions) {
            TextMeshProUGUI text = Instantiate(_testItemPrefab, _testItemsParent);

            text.text = question.Question;
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_testItemsParent as RectTransform);
    }
}