using UnityEngine;

namespace FlyStayAlive.MovementPatterns
{
    public interface IMovementPattern
    {
        Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, ref float distanceTraveled, Vector3 startPosition, float moveSpeed);
        void OnDrawGizmos(Vector3 startPosition, Transform transform);
    }
}