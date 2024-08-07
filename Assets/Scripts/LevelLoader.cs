using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;


    public GameObject cellPrefab;
    public TextAsset levelJson;
    public Transform gridParent;
    public float cellSize = 1.5f;
    public Camera mainCamera;
    public Sprite type1Sprite, type2Sprite, type3Sprite; // Assign these sprites in the Unity Inspector

    private Dictionary<string, Sprite> spriteMap;
    private int totalCells; // Total number of cells that need to be correctly oriented
    private int correctCells; // Number of cells currently in the correct orientation
    public int currentLevel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        InitializeSpriteMap();
        LoadLevel();
        CenterCamera();
    }

    void InitializeSpriteMap()
    {
        spriteMap = new Dictionary<string, Sprite>
        {
            { "type1", type1Sprite },
            { "type2", type2Sprite },
            { "type3", type3Sprite }
        };
    }

    void LoadLevel()
    {
        if (levelJson == null)
        {
            Debug.LogError("Level JSON not assigned!");
            return;
        }

        LevelData data = JsonUtility.FromJson<LevelData>(levelJson.text);
        foreach (CellPosition cell in data.cells)
        {
            Vector3 position = new Vector3(cell.x * cellSize, cell.y * cellSize, 0);
            GameObject newCell = Instantiate(cellPrefab, gridParent);
            newCell.transform.localPosition = position;
            newCell.transform.localScale = new Vector3(cellSize, cellSize, cellSize);

            SpriteRenderer renderer = newCell.GetComponent<SpriteRenderer>();
            if (renderer != null && spriteMap.ContainsKey(cell.type))
            {
                renderer.sprite = spriteMap[cell.type];
            }

            var interaction = newCell.AddComponent<CellInteraction>();
            interaction.Setup(cell.targetRotations, data.rotationAngle, this);
        }
        totalCells = data.cells.Count;
        correctCells = 0; // Reset the correct count
    }

    public void CellCorrectlyOriented()
    {
        correctCells++;
        if (correctCells >= totalCells)
        {
            LevelCompleted();
        }
    }
    public void CellNotCorrectlyOriented()
    {
        correctCells--;
    }
    void LevelCompleted()
    {
        int highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached", 1);

        if (currentLevel >= highestLevelReached)
        {
            PlayerPrefs.SetInt("HighestLevelReached", currentLevel + 1);

            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score", 0) + CalculateScore());
            PlayerPrefs.Save();
        }

        
        Debug.Log("Level complete! Score: " + PlayerPrefs.GetInt("Score"));

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.LevelCompleted();
        }
        else
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }



    int CalculateScore()
    {
        return 100;
    }

    void CenterCamera()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not assigned!");
            return;
        }

        Bounds bounds = new Bounds(gridParent.GetChild(0).position, Vector3.zero);
        foreach (Transform child in gridParent)
        {
            bounds.Encapsulate(child.position);
        }

        mainCamera.transform.position = new Vector3(bounds.center.x, bounds.center.y, mainCamera.transform.position.z);
        CameraShake cameraShake = mainCamera.GetComponent<CameraShake>();
        cameraShake.originalLocalPosition = mainCamera.transform.position;
    }
}
