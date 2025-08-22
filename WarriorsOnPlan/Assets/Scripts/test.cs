using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
//using UnityEngine.SceneManagement;
using Cases;

// [CustomEditor(typeof(Button))]
public class test : MonoBehaviour {

    public void Start() {
        Debug.Log(true.ToInteger());
        Debug.Log(false.ToInteger());
        Debug.Log(0.ToBoolean());
        Debug.Log(1.ToBoolean());
        Debug.Log((-1).ToBoolean());
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            Debug.Log(gameObject + " : " + transform.localPosition + " / " + (transform as RectTransform).anchoredPosition);
        }
    }

    public void testShout() {
        Debug.Log("test");
    }
}