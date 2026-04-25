using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsSection : MonoBehaviour {
    [SerializeField]
    private Gradient _scoreGradient;

    [SerializeField]
    private Color _noScoreColor;

    [SerializeField]
    private Image _scoreImage, _highScoreImage, _middleScoreImage, _lowScoreImage;

    [SerializeField]
    private TextMeshProUGUI _scoreText, _nameText, _usernameText;

    public void LoadStats(UserDto user) {
        RefreshScore(user);

        if (_nameText != null) _nameText.text = $"{user.Name} {user.Surname}";
        if (_usernameText != null) _usernameText.text = '@' + user.Username;
    }

    private void RefreshScore(UserDto user) {
        string tasksResponse = HttpService.SendGetRequest($"{user.Username}/tasks");
        List<TaskDto> tasks = JsonSerializer.Deserialize<List<TaskDto>>(tasksResponse);
        
        int highScore = 0, middleScore = 0, lowScore = 0;
        int scoreSum = 0;

        foreach (TaskDto task in tasks) {
            if (task.Score >= 0) scoreSum += task.Score;

            if (task.Score >= 8) highScore++;
            else if (task.Score >= 5) middleScore++;
            else if (task.Score >= 0) lowScore++;
        }

        int totalScore = highScore + middleScore + lowScore;
        if (totalScore == 0) {
            _scoreText.text = "-";
            _scoreImage.color = _noScoreColor;
            _lowScoreImage.fillAmount = 0;
            _middleScoreImage.fillAmount = 0;
            _highScoreImage.fillAmount = 0;
            return;
        }

        float averageScore = (float)scoreSum / totalScore;

        _scoreText.text = Math.Round(averageScore, 2).ToString(CultureInfo.InvariantCulture);
        _scoreImage.color = _scoreGradient.Evaluate(averageScore / 10);

        _lowScoreImage.fillAmount = (float)lowScore / totalScore;
        _middleScoreImage.fillAmount = (float)middleScore / totalScore + _lowScoreImage.fillAmount;
        _highScoreImage.fillAmount = 1;
    }
}