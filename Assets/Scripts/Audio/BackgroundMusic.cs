using UnityEngine;

// Manages the playback of background music in the game.
// Implements the Singleton pattern to ensure only one instance exists throughout the game.
public class BackgroundMusic : MonoBehaviour
{
    // Singleton instance of the BackgroundMusic class
    public static BackgroundMusic instance;
    
    // Reference to the AudioSource component that will play the music
    private AudioSource audioSource;
    
    
    // Called when the script instance is being loaded.
    // Sets up the singleton instance and initializes the AudioSource
    private void Awake()
    {
        // Ensure only one instance of BackgroundMusic exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            
            // Make sure the audio loops
            if (audioSource != null)
            {
                audioSource.loop = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Call this to start playing the music
    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    
    // Call this to stop the music
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
