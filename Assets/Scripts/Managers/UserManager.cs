using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;

public class UserManager : Singleton<UserManager> {
    [SerializeField]
    private TMP_InputField _regUsernameInput, _regPasswordInput, _regNameInput, _regSurnameInput;

    [SerializeField]
    private TMP_InputField _loginUsernameInput, _loginPasswordInput;

    [SerializeField]
    private GameObject _studentMenu, _registerMenu;

    [SerializeField]
    private TextMeshProUGUI[] _nameTexts;

    public UserDto User;

    private void Start() {
        _studentMenu.SetActive(false);
        _registerMenu.SetActive(true);
        
        User = JsonSerializer.Deserialize<UserDto>(PlayerPrefs.GetString("User"));
        
        UserDto user;
        if (User == null) user = null;
        else user = TryLogin(User.Username, User.Password);

        if (user == null) {
            User = null;
            PlayerPrefs.DeleteKey("User");
        } else {
            User = user;
            OpenStudent();
        }
    }

    public void Register() {
        string password = _regPasswordInput.text;
        
        object registerData = new {
            username = _regUsernameInput.text,
            password = BCrypt.Net.BCrypt.HashPassword(password),
            name = _regNameInput.text,
            surname = _regSurnameInput.text
        };

        string responseText = HttpService.SendPostRequest("register", registerData);
        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseText);

        if (responseData["error"].GetString() != string.Empty) { } else {
            User = JsonSerializer.Deserialize<UserDto>(responseData["user"].GetRawText());
            User.Password = password;
            
            OpenStudent();
        }
    }

    public void Login() {
        UserDto user = TryLogin(_loginUsernameInput.text, _loginPasswordInput.text);

        if (user == null) { } else {
            User = user;
            OpenStudent();
        }
    }

    private UserDto TryLogin(string username, string password) {
        object loginData = new {
            username,
            password
        };
        
        string responseText = HttpService.SendPostRequest("login", loginData);
        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseText);

        if (responseData["error"].GetString() != string.Empty) {
            return null;
        }
        
        UserDto user = JsonSerializer.Deserialize<UserDto>(responseData["user"].GetRawText());
        user.Password = password;
        return user;
    }

    private void OpenStudent() {
        PlayerPrefs.SetString("User", JsonSerializer.Serialize(User));

        string tasksResponse = HttpService.SendGetRequest($"{User.Username}/tasks");
        List<TaskDto> tasks = JsonSerializer.Deserialize<List<TaskDto>>(tasksResponse);
        
        TasksManager.Instance.LoadTasks(tasks);
        
        _registerMenu.SetActive(false);
        _studentMenu.SetActive(true);

        _regUsernameInput.text = string.Empty;
        _regPasswordInput.text = string.Empty;
        _regNameInput.text = string.Empty;
        _regSurnameInput.text = string.Empty;

        _loginUsernameInput.text = string.Empty;
        _loginPasswordInput.text = string.Empty;

        foreach (TextMeshProUGUI text in _nameTexts) {
            text.text = text.text.Replace("{name}", User.Name).Replace("{surname}", User.Surname);
        }
    }

    public void OpenRegister() {
        _regUsernameInput.text = _loginUsernameInput.text;
        _regPasswordInput.text = string.Empty;
    }

    public void OpenLogin() {
        _loginUsernameInput.text = _regUsernameInput.text;
        _loginPasswordInput.text = string.Empty;
    }
}