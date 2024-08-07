using UnityEngine;
using System.Collections.Generic;

public class CellInteraction : MonoBehaviour
{
    public float rotationAngle = 90f;
    public List<int> targetRotations = new List<int>();
    private LevelLoader levelLoader; 
    private AudioSource audioSource;
    public bool NotMatch;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CheckCompletion();

    }
    public void Setup(List<int> targetRotations, float rotationAngle, LevelLoader loader)
    {
        this.targetRotations = targetRotations;
        this.rotationAngle = rotationAngle;
        this.levelLoader = loader;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                Collider2D collider = Physics2D.OverlapPoint(touchPosition);
                if (collider != null && collider.gameObject == gameObject)
                {
                    RotateCell();
                    PlaySound();
                    CheckCompletion();
                }
            }
        }
    }

    void RotateCell()
    {
        transform.Rotate(new Vector3(0, 0, rotationAngle));
    }

    void CheckCompletion()
    {
        float currentRotation = transform.eulerAngles.z % 360;
        foreach (int targetRotation in targetRotations)
        {
            if (Mathf.Approximately(currentRotation, targetRotation))
            {
                Debug.Log("Correct rotation achieved for this cell.");
                levelLoader.CellCorrectlyOriented(); 
                NotMatch = true;
                return;
            }
            else
            {

                if(NotMatch == true)
                {
                    levelLoader.CellNotCorrectlyOriented();
                }
                NotMatch = false;

            }
        }
    }


    private void PlaySound()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
