using UnityEngine;

// Manages the playback of score-related sound effects in the game.
// Implements the Singleton pattern to ensure only one instance exists throughout the game.
public class ScoreSound : MonoBehaviour
{
    // Singleton instance of the ScoreSound class
    public static ScoreSound instance;
    
    // Reference to the AudioSource component that will play the sound
    private AudioSource audioSource;
    
    // Called when the script instance is being loaded.
    // Sets up the singleton instance and initializes the AudioSource
    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set this as the instance and make it persistent between scenes
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Get the AudioSource component attached to this GameObject
            audioSource = GetComponent<AudioSource>();
            
            // Configure audio settings - ensure it doesn't loop by default
            if (audioSource != null)
            {
                audioSource.loop = false;
            }
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
    
    // Starts playing the score sound effect if it's not already playing  
    public void PlayMusic()
    {
        // Check if we have a valid AudioSource and it's not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    
    // Stops the currently playing score sound effect
    public void StopMusic()
    {
        // Check if we have a valid AudioSource and it's currently playing
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
