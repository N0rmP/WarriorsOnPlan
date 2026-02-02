using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(buttonCustom))]
public class editorButtonCustom : ButtonEditor {
    // private SerializedProperty spTargetImages;
        

    /*
    protected override void OnEnable() {
        base.OnEnable();
        spTargetImages = serializedObject.FindProperty("thisImage");
    }
    */

    public override void OnInspectorGUI() {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "m_Script", "m_TargetGraphic");

        EditorGUILayout.Space();
        //EditorGUILayout.PropertyField(spTargetImages);

        serializedObject.ApplyModifiedProperties();
    }
}
