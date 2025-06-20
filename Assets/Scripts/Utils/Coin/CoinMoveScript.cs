using UnityEngine;

// Handles the movement of coins in the game
public class CoinMoveScript : MonoBehaviour
{
    public float amplitude = 2f;      // How high and low the coin will move
    public float frequency = 1f;       // How fast the coin moves up and down
    public int scoreValue = 2;         // How many points this coin is worth
    
    private Vector3 startPosition;    // Store the starting position of the coin
    private LogicScript logic;         // Reference to the game logic
    
    // Start is called before the first frame update
    void Start()
    {
        // Store the initial position of the coin
        startPosition = transform.position;
        
        // Get reference to the LogicScript
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        
        // Ensure the coin has a CircleCollider2D
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<CircleCollider2D>();
        }
        collider.isTrigger = true; // Make it a trigger so it doesn't cause physical collisions
        
        // Set the coin's tag
        gameObject.tag = "Coin";
        
        // Set the coin's layer to "Ignore Raycast" to prevent physics interactions
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        
        // Ensure the coin has a Rigidbody2D for collision detection
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        // Use BodyType2D.Kinematic instead of isKinematic
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void Update()
    {   
        // Calculate the new Y position using a sine wave
        // Mathf.Sin(Time.time * frequency) creates a value between -1 and 1
        // Multiply by amplitude to get the desired range (-2 to 2 in this case)
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        
        // Update the position while keeping the original x and z coordinates
        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
    
    // Called when this object is triggered by another 2D collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with the bird (using layer 3 which is the Player layer)
        if (collision.gameObject.layer == 3 && logic != null)
        {
            // Make sure we're not already being destroyed
            if (this == null) return;
            
            try 
            {
                // Increase the score by 2
                logic.addScore(scoreValue);
                
                // Play collect sound if available
                ScoreSound scoreSound = FindFirstObjectByType<ScoreSound>();
                if (scoreSound != null)
                {
                    scoreSound.PlayMusic();
                }
                
                // Disable the collider first to prevent multiple triggers
                var collider = GetComponent<Collider2D>();
                if (collider != null) collider.enabled = false;
                
                // Disable the renderer to make the coin disappear
                var renderer = GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = false;
                
                // Destroy the coin after a small delay
                Destroy(gameObject, 0.1f);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error in coin collection: {e.Message}");
            }
        }
    }
}
