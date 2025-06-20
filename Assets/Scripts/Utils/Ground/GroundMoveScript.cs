using UnityEngine;

// Handles the movement of the ground in the game
public class GroundMoveScript : MonoBehaviour
{
    // Speed at which the pipe moves to the left
    private float moveSpeed = 8f;

    // X position at which the pipe will be destroyed when it goes off-screen
    private float deadZone = -40f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code can be added here if needed
    }

    // Called every frame
    void Update()
    {
        // Move the pipe to the left based on moveSpeed and frame time
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Check if the pipe has moved past the dead zone
        if (transform.position.x < deadZone)
        {
            // Remove the pipe from the game to free up resources
            Destroy(gameObject);
        }
    }
}
