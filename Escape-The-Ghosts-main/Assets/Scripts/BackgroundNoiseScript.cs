using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNoiseScript : MonoBehaviour
{
    public AudioClip[] backgroundSounds;  // Array to hold your audio clips
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Check if there are any audio clips in the array
        if (backgroundSounds.Length > 0)
        {
            StartCoroutine(PlayRandomBackgroundSound());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add logic here if you want to change the background sound during runtime
    }

    IEnumerator PlayRandomBackgroundSound()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, backgroundSounds.Length);
            AudioClip randomClip = backgroundSounds[randomIndex];

            // Make sure there is an AudioSource component attached to the GameObject
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Assign the randomly selected audio clip to the AudioSource and play it
            audioSource.clip = randomClip;
            audioSource.Play();

            // You may want to set additional AudioSource properties here, such as volume, loop, etc.
            yield return new WaitForSeconds(13);
        }

    }
}
