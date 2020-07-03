using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer
{
    private LineRenderer lineRenderer;
    private Transform projectileStartPosition;
    private Vector3 target;

    private float projectileStartSpeed = 60f;
    private float angleOffset = 0f;

    private Vector3 hitPoint;

    public TrajectoryRenderer(LineRenderer renderer, Transform projectilePosition)
    {
        lineRenderer = renderer;
        projectileStartPosition = projectilePosition;
    }

    public void SetAngleOffset(float offset)
    {
        angleOffset = offset;
    }

    public void StartSimulating(Vector3 position)
    {
        target = position;
    }

    public void UpdateSimulation()
    {
        var speedVector = projectileStartPosition.transform.forward * projectileStartSpeed;

        var origin = projectileStartPosition.position;
        Vector3[] points = new Vector3[100];
        lineRenderer.positionCount = 100;
        for (int index = 0; index < points.Length; index++)
        {
            var time = index * 0.1f;

            points[index] = origin + speedVector * time + Physics.gravity * time * time / 2;
            if (points[index].y < 0)
            {
                lineRenderer.positionCount = index;
                break;
            }
        }

        hitPoint = points[lineRenderer.positionCount - 1];
        lineRenderer.SetPositions(points);
    }

    public void ChangeSpeed(float speed)
    {
        projectileStartSpeed = speed;
    }

    public void StopSimulating()
    {        
        lineRenderer.positionCount = 0;                
    }

    public Vector3 GetHitPoint()
    {
        return hitPoint;
    }
}
