using UnityEngine;

// Handles the spawning of clouds in the game
public class CloudSpawnScript : MonoBehaviour
{
    public GameObject cloud;
    private float spawnRate = 2f;  // Time in seconds between cloud spawns
    private float timer = 0;
    private float heightOffset = 12;
    private float zLayer = -2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnUpdate();
            timer = 0;
            // Add some randomness to spawn time (between 6-10 seconds)
            spawnRate = Random.Range(1f, 3f);
        }
    }

    void SpawnUpdate()
    {   
        zLayer *= -1;

        Instantiate(cloud, new Vector3(transform.position.x, Random.Range(-heightOffset/2, heightOffset), zLayer), transform.rotation);
    }
}
