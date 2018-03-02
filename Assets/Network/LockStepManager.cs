using UnityEngine;
using FixedMath;

public class LockStepManager : MonoBehaviour {

    public DWorld physics;

    //game update frequency in ms
    private const int rate = 1;

    private Fix32 accumulator;
    private Fix32 fixedDelta;

    void Awake() {
        this.accumulator = Fix32.Zero;
        this.fixedDelta = (Fix32)(rate / (float)1000); 
    }

	void Start () {
        this.physics = DWorld.Instance;
	}
	
	// Updates the physics
	void Update () {
        Fix32 delta = (Fix32)Time.deltaTime * (Fix32)1f;
        if (delta > (Fix32)0.25f)
            delta = (Fix32)0.25f;
        this.accumulator += delta;

        while (accumulator >= fixedDelta) {
            physics.Step(fixedDelta);
            accumulator -= fixedDelta;
        }
        DWorld.alpha = (float)(accumulator / fixedDelta);
	}
}
