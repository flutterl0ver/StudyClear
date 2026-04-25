using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : Singleton<UserManager> {
    [SerializeField]
    private TMP_InputField _regUsernameInput, _regPasswordInput, _regNameInput, _regSurnameInput;

    [SerializeField]
    private TMP_InputField _loginUsernameInput, _loginPasswordInput;
    
    [SerializeField]
    private GameObject _studentMenu, _registerMenu, _parentMenu, _teacherMenu, _emptyChildrenText, _emptyStudentsText;
    
    [SerializeField]
    private RectTransform _childrenContainer, _studentsContainer;

    [SerializeField]
    private ChildObject _childPrefab, _studentPrefab;

    public UserDto User;
    public int RegisterAccountType { get; set; }

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
            OpenMainScreen();
        }
    }

    public void Register() {
        string password = _regPasswordInput.text;
        
        object registerData = new {
            username = _regUsernameInput.text,
            password = BCrypt.Net.BCrypt.HashPassword(password),
            name = _regNameInput.text,
            surname = _regSurnameInput.text,
            accountType = RegisterAccountType
        };

        string responseText = HttpService.SendPostRequest("register", registerData);
        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseText);

        if (responseData["error"].GetString() != string.Empty) { } else {
            User = JsonSerializer.Deserialize<UserDto>(responseData["user"].GetRawText());
            User.Password = password;
            
            OpenMainScreen();
        }
    }

    public void Login() {
        UserDto user = TryLogin(_loginUsernameInput.text, _loginPasswordInput.text);

        if (user == null) { } else {
            User = user;
            OpenMainScreen();
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

    private void OpenMainScreen() {
        PlayerPrefs.SetString("User", JsonSerializer.Serialize(User));
        
        _registerMenu.SetActive(false);

        _regUsernameInput.text = string.Empty;
        _regPasswordInput.text = string.Empty;
        _regNameInput.text = string.Empty;
        _regSurnameInput.text = string.Empty;

        _loginUsernameInput.text = string.Empty;
        _loginPasswordInput.text = string.Empty;

        switch (User.AccountType) {
            case 0: OpenStudent(); break;
            case 1: OpenParent(); break;
            case 2: OpenTeacher(); break;
        }
    }
    
    private void OpenStudent() {
        _studentMenu.SetActive(true);
        
        string tasksResponse = HttpService.SendGetRequest($"{User.Username}/tasks");
        List<TaskDto> tasks = JsonSerializer.Deserialize<List<TaskDto>>(tasksResponse);
        
        TasksManager.Instance.LoadTasks(tasks);
    }

    private void OpenParent() {
        _parentMenu.SetActive(true);
        
        string childrenResponse = HttpService.SendGetRequest($"{User.Username}/children");
        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(childrenResponse);

        foreach (RectTransform child in _childrenContainer) {
            if (child == _emptyChildrenText.transform) continue;
            
            Destroy(child.gameObject);
        }
        
        if (responseData["error"].GetString() != string.Empty) { } else {
            List<UserDto> children = JsonSerializer.Deserialize<List<UserDto>>(responseData["users"].GetRawText());
            _emptyChildrenText.SetActive(children.Count == 0);
            
            foreach (UserDto child in children) {
                ChildObject newChild = Instantiate(_childPrefab, _childrenContainer);

                newChild.Init(child);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_childrenContainer);
        }
    }

    private void OpenTeacher() {
        _teacherMenu.SetActive(true);
        
        string studentsResponse = HttpService.SendGetRequest($"{User.Username}/children");
        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(studentsResponse);

        foreach (RectTransform student in _studentsContainer) {
            if (student == _emptyStudentsText.transform) continue;
            
            Destroy(student.gameObject);
        }
        
        if (responseData["error"].GetString() != string.Empty) { } else {
            List<UserDto> students = JsonSerializer.Deserialize<List<UserDto>>(responseData["users"].GetRawText());
            _emptyStudentsText.SetActive(students.Count == 0);
            
            foreach (UserDto student in students) {
                ChildObject newStudent = Instantiate(_studentPrefab, _studentsContainer);

                newStudent.Init(student);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_studentsContainer);
        }
    }

    public void ExitAccount() {
        User = null;
        PlayerPrefs.DeleteKey("User");
        
        _studentMenu.SetActive(false);
        _parentMenu.SetActive(false);
        _teacherMenu.SetActive(false);
        
        _registerMenu.SetActive(true);
        
        OpenLogin();
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