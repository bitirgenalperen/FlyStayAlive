using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}