using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Cases;

// [CustomEditor(typeof(Button))]
public class test : MonoBehaviour {

    public void Start() {
        GetComponent<showerCase>().setCase(gameManager.GM.MC.makeCodableObject<weaponTester>(93001, new int[5] { 1, 0, 1, 3, 1 }, null));
    }

    public void testShout() {
        Debug.Log("test");
    }
}