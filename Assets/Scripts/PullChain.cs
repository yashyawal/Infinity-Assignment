using UnityEngine;

public class PullChain : MonoBehaviour
{
    private Vector3 originalPosition;
    private float pullDistance = 0.5f; 
    public static PullChain Instance;
    public GameObject pauseMenu;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchingChain(touchPosition))
                {
                    PlaySound();
                    PullChainAction();
                }
            }
            else if (touch.phase == TouchPhase.Ended && IsTouchingChain(touchPosition))
            {
                ResetChainPosition();
                GameManager.Instance.PauseGame(); 
            }
        }
    }

    private bool IsTouchingChain(Vector2 touchPosition)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);
        return hitCollider != null && hitCollider.gameObject == gameObject;
    }

    private void PullChainAction()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - pullDistance, transform.position.z);
    }

    private void ResetChainPosition()
    {
        transform.position = originalPosition;
    }
    private void PlaySound()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
