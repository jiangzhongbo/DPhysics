using UnityEngine;
using System.Collections;
using FixedMath;
public class Shoot : MonoBehaviour
{
    public DBodyComponent main;
    public GameObject Ring;
    public Vector3 force;
    public Vector3 velocity;
    [ContextMenu("Shoot")]
    public void shoot()
    {
        main.GetDBody().AddForce(new Vector3F(force));
    }


    float g = -9.8f;
    public float z;
    public float x;
    public float y;
    public float vz;
    public float vx;
    public float vy;
    public float t = 1;


    public void Play()
    {
        Vector3 start = transform.position;
        Vector3 end = Ring.transform.position;
        z = (end.z - start.z);
        x = (end.x - start.x);
        y = (end.y - start.y);
        vz = z / t;
        vx = x / t;
        vy = (y - t * t * g / 2) / t;
        velocity = new Vector3(vx, vy, vz);
        main.GetDBody().Velocity = new Vector3F(velocity);
    }

}
