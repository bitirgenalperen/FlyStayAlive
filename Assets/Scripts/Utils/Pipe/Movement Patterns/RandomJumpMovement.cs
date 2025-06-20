using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}