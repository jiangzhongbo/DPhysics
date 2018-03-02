using FixedMath;
using UnityEngine;
public class DSphereCollider : DCollider
{
    private Vector3F center;
    private Fix32 radius;
    private DBox3DCollider boundingBox;

    /// <summary>
    /// Creates a new circle collider with the given position and radius.
    /// </summary>
    /// <param name="position">the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    public DSphereCollider(Vector3F position, Fix32 radius, bool isTrigger, bool isDebug)
        : base(ColliderType.Sphere, isTrigger, isDebug)
    {
        this.center = position;
        this.radius = radius;
        Vector3F scale = new Vector3F(radius * (Fix32)2, radius * (Fix32)2, radius * (Fix32)2);
        Vector3F euler = new Vector3F(0, 0, 0);
        boundingBox = new DBox3DCollider(center, scale, euler, isTrigger, isDebug);
    }

    /// <summary>
    /// Returns this radius.
    /// </summary>
    public Fix32 Radius {
        get { return this.radius; }
    }

    /// <summary>
    /// Returns a bounding box containing the circle, used to quickly compare with other boxes.
    /// </summary>
    public DBox3DCollider BoundingBox {
        get { return this.boundingBox; }
    }
    
    /// <summary>
    /// Gets the current center position.
    /// </summary>
    /// <returns>The center of the circle</returns>
    public override Vector3F GetPosition() {
        return this.center;
    }

    /// <summary>
    /// Set the position of the collider.
    /// </summary>
    /// <param name="pos">The position.</param>
    public override void SetPosition(Vector3F pos)
    {
        this.center = pos;
        this.boundingBox.SetPosition(pos);
    }

    /// <summary>
    /// Returns the minimum bounding box containing the current object.
    /// </summary>
    /// <returns>a box collider containing the current object</returns>
    public override DBox3DCollider GetContainer() {
        return this.boundingBox;
    }

    /// <summary>
    /// Transforms the position of the circle by the given amount, 
    /// together wuth the corresponding bounding box.
    /// </summary>
    /// <param name="translation">vector representing the translation.</param>
    public override void Transform(Vector3F translation) {
        this.center += translation;
        this.boundingBox.Transform(translation);
    }

    /// <summary>
    /// Checks whether the given box collider intersects with this circle, calculating
    /// the collision data in that case.
    /// </summary>
    /// <param name="other">box collider to check</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns>true if the colliders intersect, false otherwise.</returns>
    public override bool Intersects(DBox3DCollider other, out Manifold intersection) {
        intersection = null;
        return other.Intersects(this, out intersection);
    }

    /// <summary>
    /// Checks whether the given circle intersects with this circle, calculating the 
    /// collision data in that case.
    /// </summary>
    /// <param name="other">the circle to check</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns>true if the colliders intersect, false otherwise.</returns>
    public override bool Intersects(DSphereCollider other, out Manifold intersection) {
        intersection = null;
        Fix32 rDistance = this.radius + other.radius;
        Fix32 sqrRadiusDistance = rDistance * rDistance;
        Vector3F centerDistance = other.center - this.center;
        if (centerDistance.SqrtMagnitude > sqrRadiusDistance)
            return false;

        //check if one of them is a trigger
        if (this.IsTrigger || other.IsTrigger) {
            intersection = new Manifold(this.Body, other.Body);
            return true;
        }

        Fix32 distance = centerDistance.Magnitude;
        //Debug.Log("distance:" + (double)distance);
        Vector3F normal;
        Fix32 penetration;
        if (distance > Fix32.Zero) {
            penetration = rDistance - distance;
            normal = centerDistance / distance;
        }
        else {
            penetration = this.radius;
            normal = new Vector3F((Fix32)1, (Fix32)0, (Fix32)0);
        }
        intersection = new Manifold(this.Body, other.Body, normal, penetration);
        return true;
    }

    /// <summary>
    /// REturns a string containing the center and the radius of this collider.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return "Circle collider - center: " + center + ", radius: " + radius;
    }
}
