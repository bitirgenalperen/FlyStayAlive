using UnityEngine;

// Handles the movement of clouds in the game
public class CloudMoveScript : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float deadZone = -40f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
