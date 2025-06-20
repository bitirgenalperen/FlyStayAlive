using UnityEngine;

// Handles the spawning of pipes in the game
public class GroundSpawnScript : MonoBehaviour
{
    // Prefab of the pipe to be spawned
    public GameObject ground;
    
    // Time in seconds between each pipe spawn
    private float spawnRate = 2f;
    
    // Timer to track when to spawn the next pipe
    private float timer = 0;

    // Set zLayer to a negative value to ensure ground appears in front of pipes
    // Lower values (more negative) will be in front of higher values
    private float zLayer = -1f;

    // Called when the script instance is being loaded
    void Start()
    {
        // Spawn the first pipe immediately when the game starts
        SpawnUpdate();
        Instantiate(ground, new Vector3(transform.position.x-64, transform.position.y, zLayer), transform.rotation);
        Instantiate(ground, new Vector3(transform.position.x-48, transform.position.y, zLayer), transform.rotation);
        Instantiate(ground, new Vector3(transform.position.x-32, transform.position.y, zLayer), transform.rotation);
        Instantiate(ground, new Vector3(transform.position.x-16, transform.position.y, zLayer), transform.rotation);
    }

    // Called every frame
    void Update()
    {
        // Check if enough time has passed to spawn a new pipe
        if (timer < spawnRate)
        {
            // Increment the timer by the time since last frame
            timer += Time.deltaTime;
        }
        else
        {
            // Time to spawn a new pipe
            SpawnUpdate();
            // Reset the timer
            timer = 0;
        }
    }

    // Spawns a new pipe at a random height
    void SpawnUpdate()
    {
        // Create a new pipe at a random Y position within the defined range
        // The pipe is instantiated at the spawner's X position and with the same rotation
        Instantiate(ground, new Vector3(transform.position.x, transform.position.y, zLayer), transform.rotation);
    }
}
