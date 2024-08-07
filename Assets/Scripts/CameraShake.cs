using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Vector3 originalLocalPosition;

    void Awake()
    {
        originalLocalPosition = transform.localPosition;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalLocalPosition.x + x, originalLocalPosition.y + y, originalLocalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalLocalPosition;
    }
}
