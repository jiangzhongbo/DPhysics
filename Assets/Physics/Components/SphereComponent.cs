using UnityEngine;
using FixedMath;

public class SphereComponent : ColliderComponent
{

    public Vector3 center;
    public float radius;

    /// <summary>
    /// Draws the collider as a sphere.
    /// </summary>
    void OnDrawGizmos()
    {
        if (radius <= 0 || !drawCollider)
            return;
        Gizmos.color = Color.cyan;
        Vector3 pos = new Vector3(center.x, 0, center.y) + transform.position;
        Gizmos.DrawWireSphere(pos, radius);
    }

    /// <summary>
    /// Generates the "real" collider transforming the current data into fixed point data.
    /// </summary>
    /// <returns>a deterministic circle collider</returns>
    public override DCollider RequireCollider()
    {
        Vector3 global = transform.position;
        Vector3F ctr = (Vector3F)(global + center);
        return new DSphereCollider(ctr, (Fix32)radius, isTrigger, isDebug);
    }
}
