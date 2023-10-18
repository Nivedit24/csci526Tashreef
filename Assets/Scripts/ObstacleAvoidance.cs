using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3 targetPosition;
    public float minimumDistanceToObstacle = 10.0f;
    public LayerMask obstacleLayer;

    void Update()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 currentPosition = positions[i];

            // Calculate the direction and distance to the target
            Vector3 direction = (targetPosition - currentPosition).normalized;
            float distanceToTarget = Vector3.Distance(currentPosition, targetPosition);

            // Cast a ray to check for obstacles
            RaycastHit hit;
            if (Physics.Raycast(currentPosition, direction, out hit, distanceToTarget, obstacleLayer))
            {
                float adjustedDistance = hit.distance - minimumDistanceToObstacle;
                if (adjustedDistance > 0)
                {
                    positions[i] = currentPosition + direction * adjustedDistance;
                }
            }
        }

        // Update the LineRenderer positions
        lineRenderer.SetPositions(positions);
    }
}
