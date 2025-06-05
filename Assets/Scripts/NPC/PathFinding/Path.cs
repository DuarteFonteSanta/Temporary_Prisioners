using UnityEngine;

public class Path
{
    public readonly Vector2[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;

    public Path(Vector2[] waypoints, Vector2 startPosition, float turnDistance, float stoppingDistance)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = lookPoints[i];
            Vector2 dirToCurrentPoint = (currentPoint - startPosition).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDistance;
            turnBoundaries[i] = new Line(turnBoundaryPoint, startPosition - dirToCurrentPoint * turnDistance);
            startPosition = turnBoundaryPoint;
        }

        float distanceFromEndPoint = 0;

        for (int i = lookPoints.Length - 1; i > 0; i--)
        {
            distanceFromEndPoint += Vector2.Distance(lookPoints[i], lookPoints[i - 1]);
            if (distanceFromEndPoint > stoppingDistance)
            {
                slowDownIndex = i;
                break;
            }
        }
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector2 v in lookPoints)
        {
            Gizmos.DrawCube(v + Vector2.up, Vector2.one);
        }

        Gizmos.color = Color.white;
        foreach (Line l in turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}