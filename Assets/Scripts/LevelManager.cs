using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int currentLevel = 1;
    private int totalScore = 0;

    public void CompleteLevel()
    {
        totalScore += CalculateScore();
        SaveProgress();
        LoadNextLevel();
    }

    private int CalculateScore()
    {
        // Calculate score based on time taken, number of moves, etc.
        return 100; // Placeholder
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.SetInt("Score", totalScore);
    }

    private void LoadNextLevel()
    {
        currentLevel++;
    }
}
