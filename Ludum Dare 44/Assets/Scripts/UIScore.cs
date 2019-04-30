using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreText = null;

    [SerializeField]
    private TextMeshProUGUI BestText = null;

    [SerializeField]
    private TextMeshProUGUI CountText = null;

    [SerializeField]
    private TextMeshProUGUI LevelText = null;

    private void Start()
    {
        SetScoreTexts();

        ScoreManager.ScoreChanged += ScoreManager_ScoreChanged;
    }

    private void ScoreManager_ScoreChanged()
    {
        SetScoreTexts();
    }

    private void OnDestroy()
    {
        ScoreManager.ScoreChanged -= ScoreManager_ScoreChanged;
    }

    private void SetScoreTexts()
    {
        if (ScoreText == null)
            return;

        ScoreText.text = "Score: " + ScoreManager.Score.ToString("00000");
        BestText.text = "Best: " + ScoreManager.HighScore.ToString("00000");

        if (CountText != null)
            CountText.text = "Count: " + ScoreManager.TetrominoCount.ToString("00000");

        if (LevelText != null)
            LevelText.text = "Level: " + ScoreManager.Level.ToString("00000");
    }
}
