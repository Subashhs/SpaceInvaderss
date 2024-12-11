using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;  // The background music to play
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Start playing the background music and loop it
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;  // Make sure the music loops
            audioSource.Play();
        }
    }
}
