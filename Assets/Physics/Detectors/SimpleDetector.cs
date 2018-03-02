using UnityEngine;
using System.Collections.Generic;
using FixedMath;

public class SimpleDetector : ICollisionDetector
{
    private DBody main;
    private List<DBody> bodyList;

    public SimpleDetector()
    {
        bodyList = new List<DBody>();
    }

    public void Insert(DBody obj) {
        bodyList.Add(obj);
    }

    public void InsertMain(DBody obj)
    {
        this.main = obj;
    }

    public void Remove(DBody obj) {
        if (main == obj)
        {
            main = null;
        }
        else if(bodyList.Contains(obj))
        {
            bodyList.Remove(obj);
        }
    }

    public void FindCollisions(HashSet<Manifold> contacts) {
        if (main == null || bodyList.Count < 1) return;
        Profiler.BeginSample("Collision detection");
        for (int i = 0; i < bodyList.Count; i++)
        {

            //if (bodyList[i].InvMass == Fix32.Zero)
            //    continue;

            Manifold intersection;
            if (bodyList[i].Collider.Intersects(main.Collider, out intersection))
            {
                contacts.Add(intersection);
            }
        }
        Profiler.EndSample();
    }

    public void Build(List<DBody> bodies) {
        Clear();
        if (bodies.Count < 1) return;
        InsertMain(bodies[0]);
        for (int i = 1; i < bodies.Count; i++)
        {
            Insert(bodies[i]);
        }
    }

    public void Clear() {
        main = null;
        bodyList.Clear();
    }

    public void Draw() {
        
    }
}
