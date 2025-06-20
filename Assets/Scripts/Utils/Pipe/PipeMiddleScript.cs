using UnityEngine; // Core Unity functionality

// Handles scoring when the bird passes through the gap between pipes
public class PipeMiddleScript : MonoBehaviour
{
    // Reference to the game's logic controller for updating the score
    public LogicScript logic;
    // Called when the script instance is being loaded
    void Start()
    {
        // Find and store reference to the game's logic controller
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    // Not used in this script but kept for potential future functionality
    void Update()
    {
        // This method is intentionally left empty as it's not needed for the current functionality
    }

    // Called when another collider enters the trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is on layer 3 (typically the Player/Bird layer)
        if (collision.gameObject.layer == 3)
        {
            // Add 1 point to the score when the bird passes through the pipe gap
            logic.addScore(1);
        }
    }
}
