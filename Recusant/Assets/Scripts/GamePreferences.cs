using UnityEngine;

public static class GamePreferences
{
    public static void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save();
    }

    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }
}
