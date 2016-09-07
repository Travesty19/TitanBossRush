using UnityEngine;
using System.Collections;

public static class GizmoTools
{
    public static void DrawLine(Vector3 point, Vector3 up, float height, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(point, point + up * height);
    }

}
