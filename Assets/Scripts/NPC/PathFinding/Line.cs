using UnityEngine;

public struct Line
{
    private const float verticalLineGradient = 1e5f;
    private float gradient;
    private float yIntercept;
    private float gradientPerpendicular;

    private Vector2 pointOnLine1;
    private Vector2 pointOnLine2;

    private bool approachSide;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if (dx == 0)
            gradientPerpendicular = verticalLineGradient;
        else
            gradientPerpendicular = dy / dx;

        if (gradientPerpendicular == 0)
            gradient = verticalLineGradient;
        else
            gradient = -1 / gradientPerpendicular;

        yIntercept = pointOnLine.y - gradient * pointOnLine.x;
        pointOnLine1 = pointOnLine;
        pointOnLine2 = pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);
    }

    private bool GetSide(Vector2 p) => (p.x - pointOnLine1.x) * (pointOnLine2.y - pointOnLine1.y)
        > (p.y - pointOnLine1.y) * (pointOnLine2.x - pointOnLine1.x);

    public bool HasCrossedLine(Vector2 p) => GetSide(p) != approachSide;

    public float DistanceFromPoint(Vector2 point)
    {
        float yInterceptPerpendicular = point.y - gradientPerpendicular * point.x;
        float intercectX = (yInterceptPerpendicular - yIntercept) / (gradient - gradientPerpendicular);
        float intercectY = gradient * intercectX + yIntercept;
        return Vector2.Distance(point, new Vector2(intercectX, intercectY));
    }

    public void DrawWithGizmos(float lenght)
    {
        Vector2 lineDir = new Vector2(1, gradient).normalized;
        Vector2 lineCenter = new Vector2(pointOnLine1.x, pointOnLine1.y) + Vector2.up;

        Gizmos.DrawLine(lineCenter - lineDir * lenght / 2, lineCenter + lineDir * lenght / 2);
    }
}