using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;  // The background music to play
    public float musicVolume = 1.0f;   // Volume control (0.0 to 1.0)

    private AudioSource audioSource;

    private void Awake()
    {
        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
      
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        // Start playing the background music and loop it
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;  // Make sure the music loops
            audioSource.volume = musicVolume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music or AudioSource is not assigned.");
        }
    }

    // Method to change the background music dynamically
    public void ChangeMusic(AudioClip newMusic)
    {
        if (newMusic != null)
        {
            audioSource.Stop();
            audioSource.clip = newMusic;
            audioSource.Play();
        }
    }
}
