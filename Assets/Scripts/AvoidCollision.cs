using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidCollision : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LayerMask collisionLayer; // Layer containing walls and ground
    public float raycastOffset = 2.1f;

    private Vector3[] linePositions;

    void Start()
    {
        linePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePositions);
    }

    void Update()
    {
        for (int i = 1; i < linePositions.Length; i++)
        {
            Vector3 startPos = linePositions[i - 1];
            Vector3 endPos = linePositions[i];

            // Perform raycast between two points of the line
            RaycastHit hit;
            if (Physics.Linecast(startPos, endPos, out hit, collisionLayer))
            {
                // Adjust the position of the line to the collision point with an offset
                linePositions[i] = hit.point - (hit.normal * raycastOffset);
                lineRenderer.SetPositions(linePositions);
            }
        }
    }
}
