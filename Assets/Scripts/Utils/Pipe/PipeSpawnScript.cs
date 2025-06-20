using UnityEngine; // Core Unity functionality

// Handles the spawning of pipe obstacles at regular intervals
public class PipeSpawnScript : MonoBehaviour
{
    // Prefab of the pipe to be spawned
    public GameObject pipe;
    
    // Time in seconds between each pipe spawn
    private float spawnRate = 2f;
    
    // Timer to track when to spawn the next pipe
    private float timer = 0;
    
    // Maximum vertical offset for randomizing pipe height
    private float heightOffset = 6f;

    // Called when the script instance is being loaded
    void Start()
    {
        // Spawn the first pipe immediately when the game starts
        SpawnUpdate();
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
        // Calculate the minimum and maximum Y positions for pipe spawning
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        // Create a new pipe at a random Y position within the defined range
        // The pipe is instantiated at the spawner's X position and with the same rotation
        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}
