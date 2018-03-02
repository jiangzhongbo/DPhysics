using UnityEngine;
using FixedMath;

namespace FixedMath
{

    /// <summary>
    /// Struct representing a point in 3D space, using fixed point math.
    /// </summary>
    public struct Vector3F
    {
        public Fix32 x;
        public Fix32 y;
        public Fix32 z;

        private static Vector3F zero = new Vector3F(0, 0, 0);
        private static Vector3F one = new Vector3F(1, 1, 1);
        private static Vector3F down = new Vector3F(0, -1, 0);
        private static Vector3F up = new Vector3F(0, 1, 0);

        public Vector3F(Fix32 x, Fix32 y, Fix32 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3F(int x, int y, int z)
        {
            this.x = (Fix32)x;
            this.y = (Fix32)y;
            this.z = (Fix32)z;
        }

        public Vector3F(Fix32 value)
        {
            this.x = value;
            this.y = value;
            this.z = value;
        }

        public Vector3F(Vector3 v)
        {
            this.x = (Fix32)v.x;
            this.y = (Fix32)v.y;
            this.z = (Fix32)v.z;
        }

        public static Vector3F Zero
        {
            get { return zero; }
        }

        public static Vector3F One
        {
            get { return one; }
        }

        public static Vector3F Down
        {
            get { return down; }
        }

        public static Vector3F Up
        {
            get { return up; }
        }

        public Fix32 Magnitude
        {
            get
            {
                return Fix32.Sqrt(SqrtMagnitude);
            }
        }

        public Fix32 SqrtMagnitude
        {
            get
            {
                return DistanceSquared(this, zero);
            }
        }

        public Vector3F Normalized
        {
            get
            {
                Vector3F v = new Vector3F(this.x, this.y, this.z);
                v.Normalize();
                return v;
            }
        }

        public static bool operator ==(Vector3F v1, Vector3F v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
        }

        public static bool operator !=(Vector3F v1, Vector3F v2)
        {
            return !(v1 == v2);
        }

        public static Vector3F operator +(Vector3F v1, Vector3F v2)
        {
            v1.x += v2.x;
            v1.y += v2.y;
            v1.z += v2.z;
            return v1;
        }

        public static Vector3F operator -(Vector3F v1, Vector3F v2)
        {
            v1.x -= v2.x;
            v1.y -= v2.y;
            v1.z -= v2.z;
            return v1;
        }

        public static Vector3F operator -(Vector3F v)
        {
            return new Vector3F(-v.x, -v.y, -v.z);
        }

        public static Vector3F operator *(Vector3F v1, Fix32 scale)
        {
            v1.x *= scale;
            v1.y *= scale;
            v1.z *= scale;
            return v1;
        }

        public static Vector3F operator *(Vector3F v1, int scale)
        {
            v1.x *= (Fix32)scale;
            v1.y *= (Fix32)scale;
            v1.z *= (Fix32)scale;
            return v1;
        }

        public static Vector3F operator *(Vector3F v1, float scale)
        {
            Fix32 s = (Fix32)scale;
            v1.x *= s;
            v1.y *= s;
            v1.z *= s;
            return v1;
        }

        public static Vector3F operator *(Vector3F v1, Vector3F v2)
        {
            v1.x *= v2.x;
            v1.y *= v2.y;
            v1.z *= v2.z;
            return v1;
        }

        public static Vector3F operator /(Vector3F v1, Fix32 scale)
        {
            v1.x /= scale;
            v1.y /= scale;
            v1.z /= scale;
            return v1;
        }

        public static Vector3F operator /(Vector3F v1, int scale)
        {
            v1.x /= (Fix32)scale;
            v1.y /= (Fix32)scale;
            v1.z /= (Fix32)scale;
            return v1;
        }

        public static Vector3F operator /(Vector3F v1, Vector3F v2)
        {
            v1.x /= v2.x;
            v1.y /= v2.y;
            v1.z /= v2.z;
            return v1;
        }

        public static explicit operator Vector3F(Vector3 v)
        {
            return new Vector3F(
                (Fix32)v.x,
                (Fix32)v.y,
                (Fix32)v.z
                );
        }

        /// <summary>
        /// Returns a vector containing the absolute value of its components.
        /// </summary>
        public static Vector3F Abs(Vector3F v)
        {
            return new Vector3F(Fix32.Abs(v.x), Fix32.Abs(v.y), Fix32.Abs(v.z));
        }

        /// <summary>
        /// Clamps the x and y components of the given vector inside the min and max components.
        /// </summary>
        /// <param name="value">the vector to clamp</param>
        /// <param name="min">min x and y</param>
        /// <param name="max">max x and y</param>
        /// <returns>clamped vector</returns>
        public static Vector3F Clamp(Vector3F value, Vector3F min, Vector3F max)
        {
            value.x = Fix32.Clamp(value.x, min.x, max.x);
            value.y = Fix32.Clamp(value.y, min.y, max.y);
            value.z = Fix32.Clamp(value.z, min.z, max.z);
            return value;
        }

        /// <summary>
        /// Returns the euclidean distance between two coordinates.
        /// Note: Heavier than DistanceSquared, because of the Sqrt call.
        /// </summary>
        /// <param name="v1">first position</param>
        /// <param name="v2">second position</param>
        /// <returns>distance between the points</returns>
        public static Fix32 Distance(Vector3F v1, Vector3F v2)
        {
            return Fix32.Sqrt(DistanceSquared(v1, v2));
        }

        /// <summary>
        /// Returns the squared distance between the points.
        /// Use this function when the actual distance value is not important.
        /// </summary>
        /// <param name="v1">first position</param>
        /// <param name="v2">second position</param>
        /// <returns>squared distance between the two</returns>
        public static Fix32 DistanceSquared(Vector3F v1, Vector3F v2)
        {
            Fix32 x = v1.x - v2.x;
            Fix32 y = v1.y - v2.y;
            Fix32 z = v1.z - v2.z;
            return (x * x + y * y + z * z);
        }

        /// <summary>
        /// Calculates the dot product between the two vectors.
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>value indicating the dot product.</returns>
        public static Fix32 Dot(Vector3F v1, Vector3F v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// Creates a new vector using the maximum value components of the given vectors.
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>vector containing max x and y from the other two</returns>
        public static Vector3F Max(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(
                Fix32.Max(v1.x, v2.x),
                Fix32.Max(v1.y, v2.y),
                Fix32.Max(v1.z, v2.z)
                );
        }

        /// <summary>
        /// Creates a new vector using the minimum value components of the given vectors.
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>vector containing min x and y from the other two</returns>
        public static Vector3F Min(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(
                Fix32.Min(v1.x, v2.x),
                Fix32.Min(v1.y, v2.y),
                Fix32.Min(v1.z, v2.z)
                );
        }

        /// <summary>
        /// Normalizes this vector, using the inverse square root for faster calculations.
        /// </summary>
        public void Normalize()
        {
            Fix32 sqrMag = this.SqrtMagnitude;
            Fix32 invMag = (sqrMag > Fix32.Zero) ? Fix32.InvSqrt(sqrMag) : Fix32.Zero;
            this.x *= invMag;
            this.y *= invMag;
            this.z *= invMag;
        }

        /// <summary>
        /// Returns the Vector2 (float) equivalent of this vector.
        /// </summary>
        /// <returns>Vector2 converted</returns>
        public Vector3 ToVector3()
        {
            float a = (float)x;
            float b = (float)y;
            float c = (float)z;
            return new Vector3(a, b, c);
        }

        /// <summary>
        /// Checks whether the given object is a vector2F and if it has the same component values
        /// of this one.
        /// </summary>
        /// <param name="obj">the given object</param>
        /// <returns>true if it's a vector with the same component values, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return (obj is Vector3F) ? (this == (Vector3F)obj) : false;
        }

        /// <summary>
        /// Gets the hashcode of this vector.
        /// </summary>
        /// <returns>the sum of the x and y hashcodes.</returns>
        public override int GetHashCode()
        {
            return (x.GetHashCode() + y.GetHashCode()+ z.GetHashCode());
        }

        /// <summary>
        /// Returns a string containing the x and y components of this vector.
        /// </summary>
        /// <returns>string of the vector</returns>
        public override string ToString()
        {
            return "(" + x + ", " + y + ","+ z +")";
        }
    }
}