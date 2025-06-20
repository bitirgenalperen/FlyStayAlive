using UnityEngine;
using System.Collections.Generic;
using System;

// Interface for all movement patterns
public interface IMovementPattern
{
    Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed);
    void OnDrawGizmos(Vector3 startPosition, Transform transform);
}

// Simple left movement pattern
[System.Serializable]
public class LinearMovement : IMovementPattern
{
    [Tooltip("Base speed multiplier for the movement")]
    public float speedMultiplier = 1f;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        distanceTraveled += moveSpeed * speedMultiplier * deltaTime;
        return new Vector3(
            startPosition.x - distanceTraveled,
            startPosition.y,
            currentPosition.z
        );
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform) { }
}

// Vertical movement pattern (elevator/piston)
[System.Serializable]
public class VerticalMovement : IMovementPattern
{
    [Tooltip("Maximum height the object will move up from its starting position")]
    public float height = 2.0f; 
    
    [Tooltip("Speed of the vertical movement")]
    public float speed = 1.5f; 
    
    [Tooltip("If true, starts moving up first. If false, starts moving down first")]
    public bool startMovingUp = true; 
    
    private bool isInitialized = false;
    private float timeOffset;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        if (!isInitialized)
        {
            // Randomize parameters on first call
            height = UnityEngine.Random.Range(1.5f, 2.5f);
            speed = UnityEngine.Random.Range(1.0f, 2.0f);
            startMovingUp = UnityEngine.Random.value > 0.5f;
            timeOffset = UnityEngine.Random.Range(0f, 2f * Mathf.PI); // Random starting point
            isInitialized = true;
        }
        
        // Calculate horizontal movement (left)
        distanceTraveled += moveSpeed * deltaTime;
        
        // Smooth sine wave movement
        float t = (Time.time * speed + timeOffset) % (2f * Mathf.PI);
        float yOffset = Mathf.Sin(t) * height;
        
        // Apply direction based on startMovingUp
        if (!startMovingUp)
        {
            yOffset = -yOffset;
        }
        
        // Clamp Y position between -6 and 6
        float newY = Mathf.Clamp(startPosition.y + yOffset, -6f, 6f);
        
        return new Vector3(
            startPosition.x - distanceTraveled,
            newY,
            startPosition.z  // Keep original Z position
        );
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = new Color(0.8f, 0.2f, 0.8f, 0.5f); // Purple with transparency
        
        // Draw the vertical range
        Vector3 topPoint = startPosition + Vector3.up * height;
        Vector3 bottomPoint = startPosition - Vector3.up * height;
        
        // Draw vertical line
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 0.5f, topPoint.y, transform.position.z),
            new Vector3(transform.position.x - 0.5f, bottomPoint.y, transform.position.z)
        );
        
        // Draw horizontal lines at top and bottom
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 1f, topPoint.y, transform.position.z),
            new Vector3(transform.position.x, topPoint.y, transform.position.z)
        );
        
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 1f, bottomPoint.y, transform.position.z),
            new Vector3(transform.position.x, bottomPoint.y, transform.position.z)
        );
    }
}

// Sine wave movement pattern
[System.Serializable]
public class SineWaveMovement : IMovementPattern
{
    [Tooltip("Minimum amplitude of the sine wave")]
    public float minAmplitude = 0.5f;
    
    [Tooltip("Maximum amplitude of the sine wave")]
    public float maxAmplitude = 2.5f;
    
    [Tooltip("Minimum frequency of the sine wave")]
    public float minFrequency = 0.5f;
    
    [Tooltip("Maximum frequency of the sine wave")]
    public float maxFrequency = 2f;
    
    [Tooltip("Minimum speed of the sine wave")]
    public float minSpeed = 0.8f;
    
    [Tooltip("Maximum speed of the sine wave")]
    public float maxSpeed = 2f;
    
    private float timeOffset;
    private bool isInitialized = false;
    private float amplitude;
    private float frequency;
    private float speed;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        if (!isInitialized)
        {
            // Randomize parameters on first call
            amplitude = UnityEngine.Random.Range(minAmplitude, maxAmplitude);
            frequency = UnityEngine.Random.Range(minFrequency, maxFrequency);
            speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
            timeOffset = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            isInitialized = true;
        }
        
        distanceTraveled += moveSpeed * deltaTime;
        float yOffset = Mathf.Sin((Time.time * speed + timeOffset) * frequency) * amplitude;
        
        // Clamp Y position between -6 and 6
        float newY = Mathf.Clamp(startPosition.y + yOffset, -6f, 6f);
        
        return new Vector3(
            startPosition.x - distanceTraveled,
            newY,
            currentPosition.z
        );
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.green;
        int segments = 50;
        float totalDistance = 10f; // Preview distance
        float segmentLength = totalDistance / segments;
        
        Vector3 prevPoint = transform.position;
        
        for (int i = 1; i <= segments; i++)
        {
            float x = transform.position.x + i * segmentLength;
            float y = startPosition.y + Mathf.Sin((Time.time * speed + (x - transform.position.x) * 0.5f) * frequency) * amplitude;
            Vector3 nextPoint = new Vector3(x, y, transform.position.z);
            
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}

// Oval movement pattern
[System.Serializable]
public class OvalMovement : IMovementPattern
{
    [Tooltip("Minimum width of the oval (x-axis)")]
    public float minWidth = 1f;
    
    [Tooltip("Maximum width of the oval (x-axis)")]
    public float maxWidth = 2.5f;
    
    [Tooltip("Minimum height of the oval (y-axis)")]
    public float minHeight = 1f;
    
    [Tooltip("Maximum height of the oval (y-axis)")]
    public float maxHeight = 2.5f;
    
    [Tooltip("Minimum speed of the oval movement")]
    public float minSpeed = 0.6f;
    
    [Tooltip("Maximum speed of the oval movement")]
    public float maxSpeed = 1.8f;
    
    private float timeOffset;
    private bool isInitialized = false;
    private float width;
    private float height;
    private float speed;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        if (!isInitialized)
        {
            // Randomize width and height separately
            width = UnityEngine.Random.Range(minWidth, maxWidth);
            height = UnityEngine.Random.Range(minHeight, maxHeight);
            
            speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
            timeOffset = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            isInitialized = true;
        }
        
        distanceTraveled += moveSpeed * deltaTime;
        float currentAngle = (Time.time * speed + timeOffset) % (2f * Mathf.PI);
        
        float xOffset = Mathf.Cos(currentAngle) * width;
        float yOffset = Mathf.Sin(currentAngle) * height;
        
        // Clamp Y position between -6 and 6
        float newY = Mathf.Clamp(startPosition.y + yOffset, -6f, 6f);
        
        return new Vector3(
            startPosition.x - distanceTraveled + xOffset,
            newY,
            currentPosition.z
        );
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.cyan;
        int segments = 20;
        float angleStep = 2f * Mathf.PI / segments;
        
        Vector3 prevPoint = startPosition + new Vector3(
            Mathf.Cos(0) * width,
            Mathf.Sin(0) * height,
            0
        );
        
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep;
            Vector3 nextPoint = startPosition + new Vector3(
                Mathf.Cos(angle) * width,
                Mathf.Sin(angle) * height,
                0
            );
            
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}

// Hover movement pattern with periodic pauses
[System.Serializable]
public class HoverMovement : IMovementPattern
{
    [Tooltip("Distance between hover points")]
    public float hoverSpacing = 8f;  // Increased from 3f to 8f for more space between hovers
    
    [Tooltip("How long to hover at each point (seconds)")]
    public float hoverDuration = 0.5f;  // Reduced from 1f to 0.5f for shorter hovers
    
    [Tooltip("Speed multiplier when moving between hover points")]
    public float moveSpeedMultiplier = 2.5f;  // Increased from 1.5f to 2.5f for faster movement
    
    [Tooltip("If true, the object will move up and down while hovering")]
    public bool verticalHover = true;
    
    [Tooltip("Height of vertical hover movement")]
    public float hoverHeight = 0.3f;  // Reduced from 0.5f to 0.3f for less vertical movement
    
    private float nextHoverX;
    private bool isHovering = false;
    private float hoverStartTime;
    private float lastHoverX;
    private bool isInitialized = false;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        if (!isInitialized)
        {
            lastHoverX = startPosition.x;
            nextHoverX = lastHoverX - hoverSpacing;
            isInitialized = true;
        }
        
        float effectiveMoveSpeed = moveSpeed;
        float newX = currentPosition.x;
        float newY = currentPosition.y;
        
        // Check if we should start hovering
        if (!isHovering && currentPosition.x <= nextHoverX)
        {
            isHovering = true;
            hoverStartTime = Time.time;
            lastHoverX = nextHoverX;
            nextHoverX -= hoverSpacing;
        }
        // Check if we should stop hovering
        else if (isHovering && (Time.time - hoverStartTime) >= hoverDuration)
        {
            isHovering = false;
        }
        
        // Apply movement or hover
        if (isHovering)
        {
            // While hovering, move very slowly or not at all
            effectiveMoveSpeed = moveSpeed * 0.1f;
            newX = lastHoverX;
            
            // Optional vertical hover effect
            if (verticalHover)
            {
                float hoverProgress = (Time.time - hoverStartTime) / hoverDuration;
                float hoverY = Mathf.Sin(hoverProgress * Mathf.PI * 2) * hoverHeight;
                newY = startPosition.y + hoverY;
            }
        }
        else
        {
            // Move at increased speed between hovers
            effectiveMoveSpeed = moveSpeed * moveSpeedMultiplier;
        }
        
        // Apply horizontal movement
        newX -= effectiveMoveSpeed * deltaTime;
        distanceTraveled += effectiveMoveSpeed * deltaTime;
        
        // Clamp Y position between -6 and 6
        newY = Mathf.Clamp(newY, -6f, 6f);
        
        return new Vector3(
            newX,
            newY,
            startPosition.z  // Keep original Z position
        );
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.5f); // Green with transparency
        
        // Draw hover points along the path
        float currentX = startPosition.x;
        int pointCount = Mathf.FloorToInt(40f / hoverSpacing) + 2; // Cover about 40 units
        
        for (int i = 0; i < pointCount; i++)
        {
            float xPos = startPosition.x - (i * hoverSpacing);
            
            // Draw a small cross at each hover point
            float size = 0.5f;
            Gizmos.DrawLine(
                new Vector3(xPos - size, startPosition.y, 0),
                new Vector3(xPos + size, startPosition.y, 0)
            );
            Gizmos.DrawLine(
                new Vector3(xPos, startPosition.y - size, 0),
                new Vector3(xPos, startPosition.y + size, 0)
            );
            
            // If vertical hover is enabled, draw the vertical range
            if (verticalHover)
            {
                Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.2f);
                Gizmos.DrawLine(
                    new Vector3(xPos, startPosition.y - hoverHeight, 0),
                    new Vector3(xPos, startPosition.y + hoverHeight, 0)
                );
                Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.5f);
            }
        }
    }
}

// Random Vertical Jumps/Flickers pattern
[System.Serializable]
public class RandomJumpMovement : IMovementPattern
{
    [Header("Jump Height")]
    [Tooltip("Minimum jump height")]
    public float minJumpHeight = 1f;
    
    [Tooltip("Maximum jump height")]
    public float maxJumpHeight = 2.5f;
    
    [Header("Timing")]
    [Tooltip("Minimum time between jumps (seconds)")]
    public float minJumpInterval = 0.5f;
    
    [Tooltip("Maximum time between jumps (seconds)")]
    public float maxJumpInterval = 2f;
    
    [Header("Movement")]
    [Tooltip("If true, the jump will be instant. If false, it will be a quick lerp.")]
    public bool randomizeJumpType = true;
    
    [Tooltip("How fast the object moves to the new position")]
    public float minJumpSpeed = 5f;
    
    [Tooltip("Maximum jump speed")]
    public float maxJumpSpeed = 10f;
    
    [Header("Visualization")]
    [Tooltip("If true, shows the possible jump range in the editor")]
    public bool showJumpRange = true;
    
    private float nextJumpTime;
    private float currentTargetY;
    private bool isJumping;
    private bool currentJumpIsInstant;
    private float currentJumpSpeed;
    private float jumpStartY;
    private float jumpStartTime;
    private bool isInitialized = false;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        if (!isInitialized)
        {
            // Initialize with random values
            if (randomizeJumpType)
            {
                currentJumpIsInstant = UnityEngine.Random.value > 0.5f;
            }
            currentJumpSpeed = UnityEngine.Random.Range(minJumpSpeed, maxJumpSpeed);
            isInitialized = true;
            ScheduleNextJump();
        }

        // Handle horizontal movement
        distanceTraveled += moveSpeed * deltaTime;
        float newX = startPosition.x - distanceTraveled;
        
        // Handle vertical jumping
        float newY = currentPosition.y;
        
        // Check if it's time to jump
        if (Time.time >= nextJumpTime && !isJumping)
        {
            isJumping = true;
            jumpStartY = currentPosition.y;
            jumpStartTime = Time.time;
            
            // Randomize jump height and direction (up or down)
            float jumpHeight = UnityEngine.Random.Range(minJumpHeight, maxJumpHeight);
            if (UnityEngine.Random.value > 0.5f) jumpHeight = -jumpHeight;
            currentTargetY = startPosition.y + jumpHeight;
            
            // If instant jump, just snap to the target position
            if (currentJumpIsInstant)
            {
                newY = currentTargetY;
                isJumping = false;
                ScheduleNextJump();
            }
        }
        else if (isJumping && !currentJumpIsInstant)
        {
            // Handle smooth jump movement
            float jumpProgress = (Time.time - jumpStartTime) * currentJumpSpeed;
            if (jumpProgress >= 1f)
            {
                // Jump complete
                newY = currentTargetY;
                isJumping = false;
                ScheduleNextJump();
            }
            else
            {
                // Smoothly interpolate to target position with ease-in-out
                float t = jumpProgress < 0.5f 
                    ? 2f * jumpProgress * jumpProgress 
                    : 1f - Mathf.Pow(-2f * jumpProgress + 2f, 2) * 0.5f;
                
                newY = Mathf.Lerp(jumpStartY, currentTargetY, t);
            }
        }
        
        return new Vector3(
            newX,
            newY,
            startPosition.z  // Keep original Z position
        );
    }
    
    private void ScheduleNextJump()
    {
        nextJumpTime = Time.time + UnityEngine.Random.Range(minJumpInterval, maxJumpInterval);
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        if (!showJumpRange) return;
        
        // Draw the possible jump range
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange with transparency
        float width = 1f;
        
        // Top boundary
        Gizmos.DrawLine(
            new Vector3(transform.position.x - width, startPosition.y + maxJumpHeight, 0),
            new Vector3(transform.position.x + width, startPosition.y + maxJumpHeight, 0)
        );
        
        // Bottom boundary
        Gizmos.DrawLine(
            new Vector3(transform.position.x - width, startPosition.y - maxJumpHeight, 0),
            new Vector3(transform.position.x + width, startPosition.y - maxJumpHeight, 0)
        );
        
        // Vertical connection lines
        Gizmos.DrawLine(
            new Vector3(transform.position.x, startPosition.y + maxJumpHeight, 0),
            new Vector3(transform.position.x, startPosition.y - maxJumpHeight, 0)
        );
        
        // Draw current target position if jumping
        if (Application.isPlaying && isJumping)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                new Vector3(transform.position.x, currentTargetY, 0),
                0.3f
            );
        }
    }
}

// Zig-zag movement pattern
[System.Serializable]
public class ZigZagMovement : IMovementPattern
{
    [Header("ZigZag Size")]
    [Tooltip("Minimum width of the zigzag pattern")]
    public float minWidth = 1f;
    
    [Tooltip("Maximum width of the zigzag pattern")]
    public float maxWidth = 3f;
    
    [Header("Movement")]
    [Tooltip("Minimum zigzags per second")]
    public float minFrequency = 0.5f;
    
    [Tooltip("Maximum zigzags per second")]
    public float maxFrequency = 2f;
    
    [Header("Style")]
    [Tooltip("Minimum sharpness of the zigzag corners")]
    [Range(1, 5)]
    public int minSharpness = 1;
    
    [Tooltip("Maximum sharpness of the zigzag corners")]
    [Range(1, 10)]
    public int maxSharpness = 5;
    
    [Tooltip("If true, the zigzag will be more rounded")]
    public bool smoothCorners = false;
    
    private float width;
    private float frequency;
    private int sharpness;
    private bool isInitialized = false;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        if (!isInitialized)
        {
            // Initialize with random values
            width = UnityEngine.Random.Range(minWidth, maxWidth);
            frequency = UnityEngine.Random.Range(minFrequency, maxFrequency);
            sharpness = UnityEngine.Random.Range(minSharpness, maxSharpness + 1);
            isInitialized = true;
        }
        
        distanceTraveled += moveSpeed * deltaTime;
        float t = Time.time * frequency;
        
        // Use triangle wave for zigzag pattern
        float zigzag = Mathf.PingPong(t, 1f);
        
        // Apply sharpness or smoothing based on settings
        if (smoothCorners)
        {
            // Smoother, more rounded zigzag
            zigzag = Mathf.SmoothStep(0f, 1f, zigzag * 2f) * 0.5f + 
                    Mathf.SmoothStep(0f, 1f, (zigzag * 2f) - 1f) * 0.5f;
        }
        else
        {
            // Sharper corners
            zigzag = Mathf.Pow(zigzag, sharpness);
        }
        
        // Convert from 0-1 range to -1 to 1 range
        zigzag = zigzag * 2f - 1f;
        
        // Calculate and clamp Y position between -6 and 6
        float newY = Mathf.Clamp(startPosition.y + zigzag * width, -6f, 6f);
        
        return new Vector3(
            startPosition.x - distanceTraveled,
            newY,
            startPosition.z  // Keep original Z position
        );
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = new Color(1f, 0.5f, 0f); // Orange
        int segments = 30;
        float totalDistance = 10f;
        float segmentLength = totalDistance / segments;
        
        Vector3 prevPoint = transform.position;
        
        for (int i = 1; i <= segments; i++)
        {
            float x = transform.position.x + i * segmentLength;
            float t = (Time.time * frequency + i * 0.1f) % 2f;
            float zigzag = Mathf.PingPong(t, 1f);
            zigzag = Mathf.Pow(zigzag, sharpness) * 2f - 1f;
            
            Vector3 nextPoint = new Vector3(
                x,
                startPosition.y + zigzag * width,
                transform.position.z  // Keep original Z position
            );
            
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}

// Figure-eight movement pattern
[System.Serializable]
public class FigureEightMovement : IMovementPattern
{
    [Header("Figure Eight Settings")]
    [Tooltip("Base width of the figure-eight")]
    public float width = 3f;
    [Tooltip("Random width variation (+/- this value)")]
    public float widthVariation = 1f;
    
    [Space(5)]
    [Tooltip("Base height of the figure-eight")]
    public float height = 2f;
    [Tooltip("Random height variation (+/- this value)")]
    public float heightVariation = 0.5f;
    
    [Space(5)]
    [Tooltip("Base speed of the movement")]
    public float speed = 1f;
    [Tooltip("Random speed variation (multiplier between 0.5 and 1.5)")]
    public float speedVariation = 0.3f;
    
    [Header("Direction Settings")]
    [Tooltip("If true, the direction will be randomly chosen between forward and backward")]
    public bool randomizeDirection = true;
    
    private float randomWidth;
    private float randomHeight;
    private float randomSpeedMultiplier;
    private int directionMultiplier = 1; // 1 for forward, -1 for backward
    
    private float timeOffset;
    private bool isInitialized = false;
    
    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed)
    {
        if (!isInitialized)
        {
            timeOffset = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            randomWidth = Mathf.Max(0.1f, width + UnityEngine.Random.Range(-widthVariation, widthVariation));
            randomHeight = Mathf.Max(0.1f, height + UnityEngine.Random.Range(-heightVariation, heightVariation));
            randomSpeedMultiplier = Mathf.Max(0.1f, 1f + UnityEngine.Random.Range(-speedVariation, speedVariation));
            directionMultiplier = randomizeDirection ? (UnityEngine.Random.value > 0.5f ? 1 : -1) : 1;
            isInitialized = true;
        }
        
        distanceTraveled += moveSpeed * deltaTime;
        float t = (Time.time * speed * randomSpeedMultiplier * directionMultiplier + timeOffset) % (2f * Mathf.PI);
        
        // Parametric equations for a figure-eight (lemniscate of Bernoulli)
        float scale = 1f / (1f + Mathf.Sin(t) * Mathf.Sin(t));
        float xOffset = Mathf.Cos(t) * randomWidth * scale;
        float yOffset = Mathf.Sin(t) * Mathf.Cos(t) * randomHeight * scale;
        
        // Clamp Y position between -6 and 6
        float newY = Mathf.Clamp(startPosition.y + yOffset, -6f, 6f);
        
        return new Vector3(
            startPosition.x - distanceTraveled + xOffset,
            newY,
            currentPosition.z
        );
    }
    
    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.cyan;
        int segments = 50;
        
        Vector3 prevPoint = transform.position;
        
        for (int i = 0; i <= segments; i++)
        {
            float t = (i / (float)segments) * Mathf.PI * 2f;
            float scale = 1f / (1f + Mathf.Sin(t) * Mathf.Sin(t));
            
            Vector3 nextPoint = new Vector3(
                transform.position.x - (i / (float)segments) * 10f,
                startPosition.y + Mathf.Sin(t) * Mathf.Cos(t) * height * scale,
                transform.position.z  // Keep original Z position
            );
            
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}

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
