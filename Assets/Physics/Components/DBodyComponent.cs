using UnityEngine;
using System.Collections;
using FixedMath;

/// <summary>
/// Monobehaviour component used to define a new physics object.
/// </summary>
public class DBodyComponent : MonoBehaviour
{

    public bool isMain = false;
    public bool isFixed = false;
    public float speed;

    public float mass;
    public float restitution;
    public float drag;

    private ColliderComponent colliderComponent;
    private DBody body;

    //TODO: remove this temporary code
    void Start()
    {
        this.colliderComponent = GetComponent<ColliderComponent>();
        body = new DBody(
            colliderComponent.RequireCollider(),
            new Vector3F(transform.position),
            (Fix32)mass,
            (Fix32)restitution,
            (Fix32)drag
            );
        if (isMain)
        {
            DWorld.Instance.AddMainObject(body);
        }
        else
        {
            DWorld.Instance.AddObject(body);
        }

        //update position
        StartCoroutine(UpdatePosition());
    }

    void Update()
    {

        //commented this part because of the coroutine
        /*if (body.IsSleeping() || body.IsFixed())
            return;

        this.transform.position = body.InterpolatedPosition();
        */
        body.SetIsFixed(isFixed);
    }

    void OnDrawGizmos()
    {
        /*if (physicsObject == null)
            return;
        Gizmos.color = (physicsObject.IsSleeping()) ? Color.green : Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one * 2);*/
    }

    public DBody GetDBody()
    {
        return body;
    }

    private IEnumerator UpdatePosition()
    {
        while (true)
        {
            if (body.IsFixed())
                yield return null;

            this.transform.position = body.InterpolatedPosition();
            yield return null;
        }
    }
}
