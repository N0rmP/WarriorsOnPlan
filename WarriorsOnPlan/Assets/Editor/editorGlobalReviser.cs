using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public static class editorGlobalReviser {
    static editorGlobalReviser() {
        EditorApplication.hierarchyChanged += REVISE;
    }

    public static void REVISE() {
        ScrollRect[] tempOBJ = GameObject.FindObjectsOfType<ScrollRect>();

        foreach (ScrollRect sr in tempOBJ) {
            sr.movementType = ScrollRect.MovementType.Clamped;
            sr.inertia = false;
            sr.scrollSensitivity = 20;
        }
    }
}
