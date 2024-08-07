using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject movingObjectPrefab;
    public Camera mainCamera;
    public TMP_Text winText;  
    public TMP_Text scoreText;  
    public string[] winMessages; 
    public static GameManager Instance;
    public bool isGamePaused = false;
   

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);  
        scoreText.text = "Score: " + score;  
    }

    public void LevelCompleted()
    {
        //int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        int score = PlayerPrefs.GetInt("Score", 0);

        TriggerCameraShake();
        ShowWinText(LevelLoader.Instance.currentLevel - 1, score);

        GameObject spawnedObject = Instantiate(movingObjectPrefab, transform.position, Quaternion.identity, transform);
        spawnedObject.transform.localPosition = Vector3.zero;
        Animator animator = spawnedObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetInteger("LevelIndex", LevelLoader.Instance.currentLevel);
        }
        else
        {   
            Debug.LogWarning("Animator component is missing on the spawned object!");
        }

        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(ChangeSceneAfterAnimation(animationDuration + 10));

        AudioSource audioSource = spawnedObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing on the spawned object!");
        }

        DisableGameplay();
    }


    private void ShowWinText(int levelIndex, int score)
    {
        if (winText != null && levelIndex < winMessages.Length)
        {
            winText.text = winMessages[levelIndex] + "\nScore: " + score;  // Display both message and score
            winText.gameObject.SetActive(true);
        }
    }

    private void DisableGameplay()
    {
        CellInteraction[] interactions = FindObjectsOfType<CellInteraction>();
        foreach (CellInteraction interaction in interactions)
        {
            interaction.enabled = false;  
        }
    }

    public void TriggerCameraShake()
    {
        float shakeDuration = 1.0f;  
        float shakeMagnitude = 0.1f;  

        CameraShake cameraShake = mainCamera.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
        }
        else
        {
            Debug.LogWarning("CameraShake component not found on the camera!");
        }
    }

    public void PauseGame()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            PullChain.Instance.pauseMenu.SetActive(true);
            Time.timeScale = 0.1f; 
        }
    }

    public void ResumeGame()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Time.timeScale = 1; 
            PullChain.Instance.pauseMenu.SetActive(false);
        }
    }

    IEnumerator ChangeSceneAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Load the next scene, or repeat the level, or go to a menu, as appropriate
        // Example: Loading a scene named "NextScene"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + (LevelLoader.Instance.currentLevel + 1));
    }
}
