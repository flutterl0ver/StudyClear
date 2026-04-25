using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;

public class ProfileScreen : ReplacingSingleton<ProfileScreen> {
    [SerializeField]
    private TextMeshProUGUI _parentText, _teacherText, _nameText, _usernameText;

    [SerializeField]
    private GameObject _emptyParentText, _emptyTeacherText;

    [SerializeField]
    private TMP_InputField _parentUsernameInput, _teacherUsernameInput;
    
    [SerializeField]
    private StatsSection _statsSection;

    private void Start() {
        if (UserManager.Instance.User.Parent != null) {
            _emptyParentText.SetActive(false);
            _parentText.text = UserManager.Instance.User.Parent.Username;
        }
        
        if (UserManager.Instance.User.Teacher != null) {
            _emptyTeacherText.SetActive(false);
            _teacherText.text = UserManager.Instance.User.Teacher.Username;
        }
    }

    protected new void OnEnable() {
        base.OnEnable();
        if(_statsSection != null) _statsSection.LoadStats(UserManager.Instance.User);
        
        _nameText.text = $"{UserManager.Instance.User.Name} {UserManager.Instance.User.Surname}";
        _usernameText.text = '@' + UserManager.Instance.User.Username;
    }

    public void EndEditingParent() => _parentUsernameInput.onEndEdit.Invoke(_parentUsernameInput.text);
    
    public void EndEditingTeacher() => _teacherUsernameInput.onEndEdit.Invoke(_teacherUsernameInput.text);

    public void TrySetParent(string parentUsername) {
        if (parentUsername == string.Empty) {
            _parentText.text = string.Empty;
            _emptyParentText.SetActive(true);
            return;
        }

        string responseText = HttpService.SendPostRequest($"{UserManager.Instance.User.Username}/set-parent", new { parentUsername });

        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseText);

        if (responseData["error"].GetString() != string.Empty) { } else {
            UserManager.Instance.User.Parent = JsonSerializer.Deserialize<UserDto>(responseData["parentUser"].GetRawText());
            _emptyParentText.SetActive(false);
            _parentText.text = parentUsername;
        }
    }
    
    public void TrySetTeacher(string teacherUsername) {
        if (teacherUsername == string.Empty) {
            _teacherText.text = string.Empty;
            _emptyTeacherText.SetActive(true);
            return;
        }

        string responseText = HttpService.SendPostRequest($"{UserManager.Instance.User.Username}/set-teacher", new { teacherUsername });

        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseText);

        if (responseData["error"].GetString() != string.Empty) { } else {
            UserManager.Instance.User.Teacher = JsonSerializer.Deserialize<UserDto>(responseData["teacherUser"].GetRawText());
            _emptyTeacherText.SetActive(false);
            _teacherText.text = teacherUsername;
        }
    }
}