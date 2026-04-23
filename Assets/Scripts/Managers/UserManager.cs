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
    private GameObject _studentMenu, _registerMenu;

    [SerializeField]
    private TextMeshProUGUI[] _nameTexts;
    
    public UserDto User;

    private string _databaseHost = "http://localhost:8080";
    
    private void Start() {
        // проверить вход
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

        if (responseData["error"].GetString() != string.Empty) {
            
        } else {
            User = JsonSerializer.Deserialize<UserDto>(responseData["user"].GetRawText());
            
            OpenStudent();
        }
    }

    private void OpenStudent() {
        _registerMenu.SetActive(false);
        _studentMenu.SetActive(true);

        foreach (TextMeshProUGUI text in _nameTexts) {
            text.text = text.text.Replace("{name}", User.Name).Replace("{surname}", User.Surname);
        }
    }
}
