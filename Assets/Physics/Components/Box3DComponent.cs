using UnityEngine;
using FixedMath;

/// <summary>
/// Monobehaviour component for a box collider.
/// </summary>
public class Box3DComponent : ColliderComponent
{

    public Vector3 Center;
    public Vector3 Scale;
    public Vector3 Euler;
    /// <summary>
    /// Draws the collider.
    /// </summary>
    void OnDrawGizmos()
    {
        if (Scale == Vector3.zero || !drawCollider)
            return;

        Vector3 position = transform.position;
        transform.position = Center;
        Vector3 x1y1z1 = position + new Vector3(0.5f * Scale.x, 0.5f * Scale.y, 0.5f * Scale.z);
        Vector3 x1y1z2 = position + new Vector3(0.5f * Scale.x, 0.5f * Scale.y, -0.5f * Scale.z);
        Vector3 x1y2z1 = position + new Vector3(0.5f * Scale.x, -0.5f * Scale.y, 0.5f * Scale.z);
        Vector3 x1y2z2 = position + new Vector3(0.5f * Scale.x, -0.5f * Scale.y, -0.5f * Scale.z);
        Vector3 x2y1z1 = position + new Vector3(-0.5f * Scale.x, 0.5f * Scale.y, 0.5f * Scale.z);
        Vector3 x2y1z2 = position + new Vector3(-0.5f * Scale.x, 0.5f * Scale.y, -0.5f * Scale.z);
        Vector3 x2y2z1 = position + new Vector3(-0.5f * Scale.x, -0.5f * Scale.y, 0.5f * Scale.z);
        Vector3 x2y2z2 = position + new Vector3(-0.5f * Scale.x, -0.5f * Scale.y, -0.5f * Scale.z);

        x1y1z1 = rotatePointAroundPivot(x1y1z1, Center, Euler);
        x1y1z2 = rotatePointAroundPivot(x1y1z2, Center, Euler);
        x1y2z1 = rotatePointAroundPivot(x1y2z1, Center, Euler);
        x1y2z2 = rotatePointAroundPivot(x1y2z2, Center, Euler);
        x2y1z1 = rotatePointAroundPivot(x2y1z1, Center, Euler);
        x2y1z2 = rotatePointAroundPivot(x2y1z2, Center, Euler);
        x2y2z1 = rotatePointAroundPivot(x2y2z1, Center, Euler);
        x2y2z2 = rotatePointAroundPivot(x2y2z2, Center, Euler);

        Gizmos.DrawLine(x1y1z1, x1y1z2);
        Gizmos.DrawLine(x1y1z1, x1y2z1);
        Gizmos.DrawLine(x1y1z1, x2y1z1);

        Gizmos.DrawLine(x2y2z2, x1y2z2);
        Gizmos.DrawLine(x2y2z2, x2y2z1);
        Gizmos.DrawLine(x2y2z2, x2y1z2);

        Gizmos.DrawLine(x1y2z1, x1y2z2);
        Gizmos.DrawLine(x1y2z1, x2y2z1);

        Gizmos.DrawLine(x2y1z2, x1y1z2);
        Gizmos.DrawLine(x2y1z2, x2y1z1);

        Gizmos.DrawLine(x1y2z2, x1y1z2);
        Gizmos.DrawLine(x2y2z1, x2y1z1);
    }

    /// <summary>
    /// Generates the "real" collider transforming the current data into fixed point data.
    /// </summary>
    /// <returns>a deterministic collider</returns>
    public override DCollider RequireCollider()
    {
        Vector3F fCenter = (Vector3F)transform.position;
        Vector3F fScale = (Vector3F)Scale;
        Vector3F fEuler = (Vector3F)Euler;
        return new DBox3DCollider(fCenter, fScale, fEuler, isTrigger, isDebug);
    }

    Vector3 rotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }
}
