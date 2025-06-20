using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}