using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public static class editorGlobalReviser {
    [MenuItem("Tools/REVISE ONCE")]
    public static void REVISE() {
        Debug.Log("test");
        TextMeshProUGUI[] tempOBJ = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI obj in tempOBJ) {
            obj.gameObject.AddComponent<localizerFont>().thisFontTableKey = enumFontTableKey.MainFont;
        }
    }
}
