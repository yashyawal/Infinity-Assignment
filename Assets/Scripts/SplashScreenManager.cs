using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField] private float delay = 3f;

    void Start()
    {
        Invoke("LoadHighestLevelReached", delay);
    }

    void LoadHighestLevelReached()
    {
        int highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached", 1);
        SceneManager.LoadScene("Level " + highestLevelReached); 
    }
}
