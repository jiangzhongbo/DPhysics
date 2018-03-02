using FixedMath;
using UnityEngine;
public class EulerImplicit : IIntegrator {

    public EulerImplicit() { }

    public void IntegrateForces(DBody body, Fix32 delta) {
        //Debug.Log("1body.Velocity:" + body.Velocity.ToVector3());
        body.Velocity += (DWorld.GRAVITY + body.Force * body.InvMass) * delta;
       // Debug.Log("2body.Velocity:" + body.Velocity.ToVector3());
        //Debug.Log("delta:" + (float)delta);
        //body.Velocity *= (Fix32.One - body.Drag);
    }

    public void IntegrateVelocities(DBody body, Fix32 delta) {
        body.Transform(body.Velocity * delta);
        body.ClearForces();
    }
}
