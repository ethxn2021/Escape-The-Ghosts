using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Intensity of the shake
    public float shakeIntensity = 0.1f;

    // Duration of the shake in seconds
    public float shakeDuration = 0.5f;

    // Whether to smooth the shake or not
    public bool smoothShake = true;

    // How quickly the shake decreases in intensity (only used if smoothShake is true)
    public float decreaseFactor = 1.0f;

    private Vector3 originalPosition;

    private void Start()
    {
        // Store the original position of the camera
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            // Generate a random offset based on the intensity
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;

            // Apply the offset to the camera's position
            if (smoothShake)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + shakeOffset, Time.deltaTime * decreaseFactor);
            }
            else
            {
                transform.localPosition = originalPosition + shakeOffset;
            }

            // Decrease the remaining shake duration
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            // Reset the camera position when the shake duration is over
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    // Call this method to start the camera shake
    public void StartShake()
    {
        shakeDuration = 0.5f; // Set the desired shake duration
    }
}
