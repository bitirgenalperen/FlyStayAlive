using UnityEngine;          // Core Unity functionality
using UnityEngine.InputSystem; // New Input System for handling input
using System.Collections;    // Basic collection types and enumerators

// Main script controlling the bird's behavior in the game
public class BirdScript : MonoBehaviour
{
    // Reference to the bird's Rigidbody2D component for physics
    public Rigidbody2D myRigidbody;
    
    // How high the bird will jump when flapping
    private float flapStrength = 10f;
    
    // Reference to the game's logic controller
    public LogicScript logic;
    
    // Tracks whether the bird is still in play
    public bool birdIsAlive = true;
    
    // Handles input from the new Input System
    private PlayerInput playerInput;
    
    // Custom action for the flap/jump input
    private InputAction flapAction;


    // Called once when the script instance is being loaded
    void Start()
    {
        // Find and store reference to the game's logic controller
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        
        // Set up the new Input System
        // Get existing PlayerInput or add one if it doesn't exist
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }
        
        // Create a new input action called "Flap" that responds to:
        // 1. Spacebar on keyboard
        // 2. Touch input on mobile devices
        flapAction = new InputAction("Flap", binding: "<Keyboard>/space");
        flapAction.AddBinding("<Touchscreen>/press");
        flapAction.Enable();  // Enable the action so it can be used
    }

    // Called every frame
    void Update()
    {
        // Check if the flap action was triggered and the bird is still alive
        if (flapAction.triggered && birdIsAlive)
        {
            // Apply upward force to make the bird flap/jump
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
        }
        
        // Check if the bird has gone out of bounds (too high or too low)
        if (transform.position.y > 14 || transform.position.y < -14)
        {
            // Trigger game over if bird is out of bounds
            logic.gameOver();
            birdIsAlive = false;
        }
    }

    // Called when the bird collides with another 2D collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't trigger game over if colliding with a coin
        if (collision.gameObject.CompareTag("Coin")) return;
        
        // Trigger game over for other collisions
        logic.gameOver();
        birdIsAlive = false;
    }
    
    // Called when the script is being disabled
    private void OnDisable()
    {
        // Clean up the input action to prevent memory leaks
        if (flapAction != null)
        {
            flapAction.Disable();  // Disable the input action
        }
    }


}
