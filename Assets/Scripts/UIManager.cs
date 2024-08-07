using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource; 

    private void Start()
    {
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleSound()
    {
        bool isMuted = PlayerPrefs.GetInt("soundMuted", 0) == 1;
        PlayerPrefs.SetInt("soundMuted", isMuted ? 0 : 1);
        PlayerPrefs.Save();
        AudioListener.volume = isMuted ? 1.0f : 0.0f;
    }

    public void ToggleMusic()
    {
        bool isMusicMuted = PlayerPrefs.GetInt("musicMuted", 0) == 1;
        PlayerPrefs.SetInt("musicMuted", isMusicMuted ? 0 : 1);
        PlayerPrefs.Save();
        backgroundMusicSource.volume = isMusicMuted ? 1.0f : 0.0f;
    }

 
}
