using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public int currentLevel = 1;
    public TMP_Text levelNumberText;
    public Button leftArrow;
    public Button rightArrow;
    private int highestLevelReached;

    void Start()
    {
       // highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached", 1);
        UpdateLevelNumber();
        UpdateArrowButtons();
    }

    public void ChangeLevel(int change)
    {
        currentLevel += change;
        if (currentLevel < 1) currentLevel = 1;
        if (currentLevel > highestLevelReached) currentLevel = highestLevelReached;

        UpdateLevelNumber();
        UpdateArrowButtons();

        StopAllCoroutines();
        StartCoroutine(DelayedLoadLevel(2f));
    }

    IEnumerator DelayedLoadLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadSelectedLevel();
    }

    void UpdateLevelNumber()
    {
        levelNumberText.text = "# " + currentLevel.ToString();
    }

    void UpdateArrowButtons()
    {
        leftArrow.interactable = currentLevel > 1;
        rightArrow.interactable = currentLevel < highestLevelReached;
    }

    public void LoadSelectedLevel()
    {
        SceneManager.LoadScene("Level " + currentLevel);
    }
}
