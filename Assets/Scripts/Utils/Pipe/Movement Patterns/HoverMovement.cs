using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}    