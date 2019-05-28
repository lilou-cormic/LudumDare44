using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Score { get; private set; }
    public static int HighScore { get; private set; }
    public static int ScoreMultiplier { get; private set; }

    public static int TetrominoCount { get; private set; }
    public static int Level { get; private set; }

    public static event Action ScoreChanged;

    private void Awake()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Start()
    {
        Score = 0;
        ScoreMultiplier = 1;
        TetrominoCount = 0;
        Level = 1;

        ScoreChanged?.Invoke();
    }

    public static int AddPoints(int points)
    {
        int pts = points * ScoreMultiplier;

        if (pts > 0)
            Score += pts;

        ScoreChanged?.Invoke();

        return pts;
    }

    public static void AddTetromino()
    {
        TetrominoCount++;

        ScoreChanged?.Invoke();
    }

    public static void AddLevel()
    {
        Level++;

        ScoreChanged?.Invoke();
    }

    public static void SetHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;

            PlayerPrefs.SetInt("HighScore", HighScore);
        }

        ScoreChanged?.Invoke();
    }

    public static void ResetScoreMultiplier()
    {
        ScoreMultiplier = 1;

        ScoreChanged?.Invoke();
    }

    public static void IncrementMultiplier()
    {
        ScoreMultiplier++;

        ScoreChanged?.Invoke();

    }
}
