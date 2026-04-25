using TMPro;
using UnityEngine;

public class ChildObject : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _nameText, _usernameText;

    private UserDto _userData;
    
    public void Init(UserDto user) {
        _nameText.text = user.Name;
        _usernameText.text = '@' + user.Username;

        _userData = user;
    }

    public void OpenParent() {
        MainScreen.Instance.OpenChildStats(_userData);
    }

    public void OpenTeacher() {
        MainScreen.Instance.OpenStudentStats(_userData);
    }
}