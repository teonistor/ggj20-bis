using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spot))]
[CanEditMultipleObjects]
public class SpotEstablishEditor : Editor {

    public override void OnInspectorGUI () {
        base.OnInspectorGUI();

        if (GUILayout.Button("Recalculate neighbours")) {
            foreach(Object target in targets) {
                ((Spot)target).RecalculateNeighbours();
            }
        }
    }
}
