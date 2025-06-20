using UnityEngine;
using System.Collections.Generic;
using System;
using FlyStayAlive.MovementPatterns;  // Import the namespace of the movement patterns

// Handles the movement and cleanup of pipe obstacles with various movement patterns
public class PipeMoveScript : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Base speed at which the pipe moves to the left")]
    [SerializeField] private float moveSpeed = 8f;
    
    [Header("Movement Patterns")]
    [SerializeField] private List<IMovementPattern> movementPatterns = new List<IMovementPattern>
    {
        new LinearMovement(), // TESTED
        new VerticalMovement(), // TESTED
        new SineWaveMovement(), // TESTED
        new OvalMovement(), // TESTED
        new HoverMovement(), // TESTED
        new RandomJumpMovement(), // TESTED
        new ZigZagMovement(), // TESTED
        new FigureEightMovement() // TESTED
    };
    
    [Tooltip("Current movement pattern index")]
    [SerializeField] private int currentPatternIndex = 0;

    // X position at which the pipe will be destroyed when it goes off-screen
    private float deadZone = -40f;
    
    // Movement state
    private Vector3 startPosition;
    private float distanceTraveled;
    private IMovementPattern currentPattern;

    private void Start()
    {
        startPosition = transform.position;
        
        // Select a random pattern if there are patterns available
        if (movementPatterns != null && movementPatterns.Count > 0)
        {
            currentPatternIndex = UnityEngine.Random.Range(0, movementPatterns.Count);
            currentPattern = movementPatterns[currentPatternIndex];
        }
    }

    private void Update()
    {
        if (currentPattern == null) return;
        
        // Calculate new position using the current movement pattern
        Vector3 newPosition = currentPattern.CalculateMovement(
            transform.position,
            Time.deltaTime,
            ref distanceTraveled,
            startPosition,
            moveSpeed
        );
        
        // Apply the new position
        transform.position = newPosition;

        // Check if the pipe has moved past the dead zone
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (currentPattern != null && Application.isPlaying)
        {
            currentPattern.OnDrawGizmos(startPosition, transform);
        }
    }
    
    // Method to change the movement pattern at runtime
    public void SetMovementPattern(int index)
    {
        if (index >= 0 && index < movementPatterns.Count)
        {
            currentPatternIndex = index;
            currentPattern = movementPatterns[index];
            startPosition = transform.position;
            distanceTraveled = 0f;
        }
    }
    
    // Method to get all available pattern names
    public string[] GetPatternNames()
    {
        string[] names = new string[movementPatterns.Count];
        for (int i = 0; i < movementPatterns.Count; i++)
        {
            names[i] = movementPatterns[i].GetType().Name;
        }
        return names;
    }
}
