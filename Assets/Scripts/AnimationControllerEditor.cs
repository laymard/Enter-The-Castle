using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AnimationController))]
public class AnimationControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AnimationController myScript = (AnimationController)target;
        if (GUILayout.Button("Go To Target"))
        {
            myScript.GoToTarget(myScript.CurrentTarget.position);
        }
    }
}
#endif