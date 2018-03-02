using UnityEngine;
using System.Collections;
using FixedMath;

public class Test : MonoBehaviour
{
    public GameObject sphere;
    void Start()
    {
        sphere.transform.position = rotatePointAroundPivot2(sphere.transform.position, -transform.eulerAngles);
        transform.eulerAngles = Vector3.zero;
    }


    Vector3 rotatePointAroundPivot(Vector3 point, Vector3 angles)
    {
        float x = point.x;
        float y = point.y;
        float z = point.z;

        float a = angles.z * Mathf.Deg2Rad;
        float b = angles.y * Mathf.Deg2Rad;
        float c = angles.x * Mathf.Deg2Rad;

        float x1 = x * Mathf.Cos(a) - y * Mathf.Sin(a);
        float y1 = x * Mathf.Sin(a) + y * Mathf.Cos(a);

        float x2 = x * Mathf.Cos(b) - z * Mathf.Sin(b);
        float z2 = x * Mathf.Sin(b) + z * Mathf.Cos(b);

        float y3 = y * Mathf.Cos(c) - z * Mathf.Sin(c);
        float z3 = y * Mathf.Sin(c) + z * Mathf.Cos(c);

        var ret = new Vector3(x2, y3, z3);
        return ret;
    }

    Vector3 rotatePointAroundPivot2(Vector3 point, Vector3 angles)
    {
        Vector3 dir = point;
        dir = Quaternion.Euler(angles) * dir;
        point = dir;
        return point;
    }
}
