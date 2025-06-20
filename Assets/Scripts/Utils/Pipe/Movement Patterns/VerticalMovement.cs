using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}