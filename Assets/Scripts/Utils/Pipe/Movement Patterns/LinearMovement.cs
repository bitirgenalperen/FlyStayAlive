using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
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
}