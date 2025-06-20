using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}