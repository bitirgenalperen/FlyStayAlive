using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}