using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class GenRing : MonoBehaviour
{
    public int num;
    public float r = 1;
    public GameObject sphere;
    [ContextMenu("Gen")]
    public void Gen()
    {
        Clear();
        float deg = 360 / num;
        for (int i = 0; i < num; i++)
        {
            GameObject p = GameObject.Instantiate(sphere) as GameObject;
            p.transform.parent = transform;
            p.name = "Sphere_" + i;
            p.transform.localPosition = p.transform.localEulerAngles = Vector3.zero;
            p.transform.localPosition = new Vector3(Mathf.Cos(deg * i * Mathf.Deg2Rad), 0, Mathf.Sin(deg * i * Mathf.Deg2Rad)) * r;
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }

        list.ForEach(e =>
        {
            DestroyImmediate(e);
        });
    }
}
