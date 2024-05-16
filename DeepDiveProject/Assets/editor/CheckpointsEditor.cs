using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AICheckpoints))]
public class CheckpointsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AICheckpoints script = (AICheckpoints)target;

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Angle Size Checkpoint Walls") == true)
        {
            script.AngleSizeCheckpointWalls();
        }

        GUILayout.Label(script.Description());
    }
}