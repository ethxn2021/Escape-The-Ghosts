using System.Collections;
using UnityEngine;

public class SpotLightFlicker : MonoBehaviour
{
    public Light spotlight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public float flickerSpeed = 1.0f;

    void Start()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();
        }

        // Start the flickering coroutine
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Randomly change the intensity
            float targetIntensity = Random.Range(minIntensity, maxIntensity);
            float currentIntensity = spotlight.intensity;

            // Lerp between the current intensity and the target intensity
            float t = 0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime * flickerSpeed;
                spotlight.intensity = Mathf.Lerp(currentIntensity, targetIntensity, t);
                yield return null;
            }

            yield return null;
        }
    }
}
