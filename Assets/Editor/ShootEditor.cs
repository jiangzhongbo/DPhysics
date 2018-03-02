using UnityEngine;
using UnityEditor;
using FixedMath;
[CustomEditor(typeof(Shoot))]
public class ShootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Shoot"))
        {
            ((Shoot)target).GetComponent<DBodyComponent>().isFixed = false;
            ((Shoot)target).shoot();
        }

        if (GUILayout.Button("Play"))
        {
            ((Shoot)target).GetComponent<DBodyComponent>().isFixed = false;
            ((Shoot)target).Play();
        }

        if (GUILayout.Button("Reset"))
        {
            ((Shoot)target).GetComponent<DBodyComponent>().isFixed = true;
            ((Shoot)target).GetComponent<DBodyComponent>().GetDBody().ClearForces();
            ((Shoot)target).GetComponent<DBodyComponent>().GetDBody().Velocity = Vector3F.Zero;
            ((Shoot)target).GetComponent<DBodyComponent>().GetDBody().SetPosition(new Vector3F(new Vector3(0, 2, -6)));
        }
    }
}