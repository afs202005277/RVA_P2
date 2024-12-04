using UnityEngine;
using System.Collections;

public class BirdSongManager : MonoBehaviour
{
    public AudioClip song1;
    public AudioClip song2;
    public float minWaitTime = 5f; // Minimum wait time before the next song
    public float maxWaitTime = 15f; // Maximum wait time before the next song

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning($"No audio source found on {gameObject.name}.");
        }

        // Start the coroutine to handle bird singing
        StartCoroutine(SingSongs());
    }

    private IEnumerator SingSongs()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Alternate between song1 and song2 randomly
            AudioClip nextSong = Random.value > 0.5f ? song1 : song2;
            audioSource.clip = nextSong;

            // Play the chosen song
            if (audioSource.enabled)
            {
                audioSource.Play();
            }

            // Wait for the song to finish playing before continuing
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
}
