using FixedMath;
using UnityEngine;
using System;

/// <summary>
/// 3D collider representing a cuboid, using fixed point math. It is defined by the minimum and
/// maximum edges, located respectively on the bottom left and top right.
/// </summary>
public class DBox3DCollider : DCollider
{
    private Vector3F center;
    private Vector3F scale;
    private Vector3F euler;

    /// <summary>
    /// Creates a new box collider with the given dimensions.
    /// </summary>
    /// <param name="min">te minimum edge</param>
    /// <param name="max">the maximum edge</param>
    public DBox3DCollider(Vector3F center, Vector3F scale, Vector3F euler, bool isTrigger, bool isDebug)
        : base(ColliderType.Box3D, isTrigger, isDebug)
    {
        this.center = center;
        this.scale = scale;
        this.euler = euler;
    }

    /// <summary>
    /// Returns the minimum edge.
    /// </summary>
    public Vector3F Center
    {
        get { return this.center; }
    }

    /// <summary>
    /// Returns the maximum edge.
    /// </summary>
    public Vector3F Scale
    {
        get { return this.scale; }
    }

    public Vector3F Euler
    {
        get { return this.euler; }
    }

    /// <summary>
    /// Gets the collider's position, intended as the bottom left edge.
    /// </summary>
    /// <returns>The minimum edge</returns>
    public override Vector3F GetPosition()
    {
        return this.center;
    }

    /// <summary>
    /// Returns the current object as the bounding box.
    /// </summary>
    /// <returns>The current bounding box.</returns>
    public override DBox3DCollider GetContainer()
    {
        return this;
    }

    /// <summary>
    /// Gets a vector representing the extension of this collider on the x and y axis.
    /// </summary>
    /// <returns>The extents of the collider</returns>
    public Vector3F GetExtents()
    {
        return scale;
    }

    /// <summary>
    /// Set the position of the collider.
    /// </summary>
    /// <param name="pos">The position.</param>
    public override void SetPosition(Vector3F pos)
    {
        this.center = pos;
    }

    /// <summary>
    /// Transforms the collider's position by the given amount.
    /// </summary>
    /// <param name="translation">Vector indicating the translation</param>
    public override void Transform(Vector3F translation)
    {
        this.center += translation;
    }

    /// <summary>
    /// Checks whether the given colliders intersect, generating an Intersection instance
    /// with the collision data in that case.
    /// Since this function represents a "box vs circle" collision, the symmetric function is called
    /// instead. See DCircleCollider for more info.
    /// </summary>
    /// <param name="other">the second collider</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns></returns>
    public override bool Intersects(DSphereCollider other, out Manifold intersection)
    {
        intersection = null;

        Vector3F spherePos = other.GetPosition();
        Vector3F boxPos = this.GetPosition();

        spherePos = spherePos - boxPos;
        boxPos = Vector3F.Zero;


        //spherePos = rotatePointAroundPivot(spherePos, -euler);

        Vector3F h = scale / 2;
        Vector3F v = Vector3F.Abs(spherePos);
        Vector3F u = Vector3F.Max(v - h, Vector3F.Zero);
        Vector3F f = getFlag(spherePos);
        Fix32 penetration;
        Vector3F normal = Vector3F.Zero;
        bool ret = Vector3F.Dot(u, u) <= other.Radius * other.Radius;
        if (!ret)
        {
            return false;
        }
        else
        {
            if (this.IsTrigger || other.IsTrigger)
            {
                intersection = new Manifold(this.Body, other.Body);
                return true;
            }
            if (u.x > Fix32.Zero || u.y > Fix32.Zero || u.z > Fix32.Zero)
            {
                //real_u = rotatePointAroundPivot(real_u, euler);
                if (IsDebug)
                {
                    Debug.Log("u.x:" + (float)u.x);
                    Debug.Log("u.y:" + (float)u.y);
                    Debug.Log("u.z:" + (float)u.z);
                    Debug.Log("f.x:" + (float)f.x);
                    Debug.Log("f.y:" + (float)f.y);
                    Debug.Log("f.z:" + (float)f.z);
                    Debug.Log("spherePos.x:" + (float)spherePos.x);
                    Debug.Log("spherePos.y:" + (float)spherePos.y);
                    Debug.Log("spherePos.z:" + (float)spherePos.z);
                }
                normal = u / u.Magnitude;
                normal = new Vector3F(normal.x * f.x, normal.y * f.y, normal.z * f.z);
                penetration = other.Radius - u.Magnitude;
            }
            else
            {
                Vector3F vh = v - h;
                if (vh.x < vh.y && vh.x < vh.z)
                {
                    var n = new Vector3F(vh.x, Fix32.Zero, Fix32.Zero);
                    normal = n / n.Magnitude;
                    penetration = other.Radius - n.Magnitude;
                }
                else if (vh.y < vh.x && vh.y < vh.z)
                {
                    var n = new Vector3F(Fix32.Zero, vh.y, Fix32.Zero);
                    normal = n / n.Magnitude;
                    penetration = other.Radius - n.Magnitude;
                }
                else
                {
                    var n = new Vector3F(Fix32.Zero, Fix32.Zero, vh.z);
                    normal = n / n.Magnitude;
                    penetration = other.Radius - n.Magnitude;
                }
            }
            intersection = new Manifold(this.Body, other.Body, normal, penetration);
        }
        return true;
    }

    Vector3F rotatePointAroundPivot(Vector3F point, Vector3F angles)
    {
        Fix32 x = point.x;
        Fix32 y = point.y;
        Fix32 z = point.z;

        Fix32 a = angles.z * Fix32.Deg2Rad;
        Fix32 b = angles.y * Fix32.Deg2Rad;
        Fix32 c = angles.x * Fix32.Deg2Rad;

        Fix32 x1 = x * Trig.Cos(a) - y * Trig.Sin(a);
        Fix32 y1 = x * Trig.Sin(a) + y * Trig.Cos(a);

        Fix32 x2 = x * Trig.Cos(b) - z * Trig.Sin(b);
        Fix32 z2 = x * Trig.Sin(b) + z * Trig.Cos(b);

        Fix32 y3 = y * Trig.Cos(c) - z * Trig.Sin(c);
        Fix32 z3 = y * Trig.Sin(c) + z * Trig.Cos(c);

        var ret = new Vector3F(x2, y3, z3);
        return ret;
    }


    Vector3F getFlag(Vector3F v)
    {
        Vector3F f = new Vector3F();
        if (v.x < Fix32.Zero)
        {
            f.x = -Fix32.One;
        }
        else if (v.x == Fix32.Zero)
        {
            f.x = Fix32.Zero;
        }
        else
        {
            f.x = Fix32.One;
        }
        if (v.y < Fix32.Zero)
        {
            f.y = -Fix32.One;
        }
        else if (v.y == Fix32.Zero)
        {
            f.y = Fix32.Zero;
        }
        else
        {
            f.y = Fix32.One;
        }
        if (v.z < Fix32.Zero)
        {
            f.z = -Fix32.One;
        }
        else if (v.z == Fix32.Zero)
        {
            f.z = Fix32.Zero;
        }
        else
        {
            f.z = Fix32.One;
        }
        return f;
    }

    Vector3F checkRange(Vector3F v)
    {
        Fix32 b = Fix32.Create(1, 100);
        //Debug.Log("b:"+(float)b);
        if (Fix32.Zero < v.x && v.x < b)
        {
            v.x = b;
        }
        else if (Fix32.Zero < v.y && v.y < b)
        {
            v.y = b;
        }
        else if (Fix32.Zero < v.z && v.z < b)
        {
            v.z = b;
        }
        return v;
    }

    /// <summary>
    /// Checks whether the two boxes collide, generating an intersection containing the collision data.
    /// This function can only compare bounding boxes againsts other bounding boxes.
    /// </summary>
    /// <param name="other">bounding box to check</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns></returns>
    public override bool Intersects(DBox3DCollider other, out Manifold intersection)
    {
        intersection = null;
        return false;
    }

    /// <summary>
    /// Simple function used to check whether the given point lies inside the collider's
    /// boundaries.
    /// </summary>
    /// <param name="point">Vector representing a point</param>
    /// <returns>true if the point is inside the collider, false otherwise.</returns>
    public bool Contains(Vector3F point)
    {
        return false;
    }

    /// <summary>
    /// Returns a string containing the min and max vectors.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "Box collider - center: " + center + ", scale: " + scale + ", euler:" + euler;
    }
}
