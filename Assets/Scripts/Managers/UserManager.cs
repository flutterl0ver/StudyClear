using System;
using System.Collections.Generic;
using System.Net.Http;
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

    private string _databaseHost = "http://localhost:8080";

    private void Start() {
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
        string registerData = JsonSerializer.Serialize(new {
            username = _regUsernameInput.text,
            password = BCrypt.Net.BCrypt.HashPassword(_regPasswordInput.text),
            name = _regNameInput.text,
            surname = _regSurnameInput.text
        });

        HttpClient client = new();
        HttpResponseMessage response = client.PostAsync(_databaseHost + "/register", new StringContent(registerData)).Result;
        string responseText = response.Content.ReadAsStringAsync().Result;
        Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseText);

        if (responseData["error"].GetString() != string.Empty) { } else {
            User = JsonSerializer.Deserialize<UserDto>(responseData["user"].GetRawText());

            OpenStudent();
        }
    }

    public void Login() {
        UserDto user = TryLogin(User.Username, User.Password);

        if (user == null) { } else {
            User = user;
            OpenStudent();
        }
    }

    private UserDto TryLogin(string username, string password) {
        // string loginData = JsonSerializer.Serialize(new { username = _loginUsernameInput.text, password = _loginPasswordInput.text; });
        //
        // HttpClient client = new();
        // HttpResponseMessage response = client.PostAsync(_databaseHost + "/login", new StringContent(registerData)).Result;
        // string responseText = response.Content.ReadAsStringAsync().Result;
        // Dictionary<string, JsonElement> responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseText);
        //
        // if (responseData["error"].GetString() != string.Empty) { } else {
        //     User = JsonSerializer.Deserialize<UserDto>(responseData["user"].GetRawText());
        //
        //     OpenStudent();
        // }

        return null;
    }

    private void OpenStudent() {
        PlayerPrefs.SetString("User", JsonSerializer.Serialize(User));

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