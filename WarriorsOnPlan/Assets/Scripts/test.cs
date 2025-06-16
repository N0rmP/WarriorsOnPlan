using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using Unity.VisualScripting;

public class test : MonoBehaviour {
    public static Vector3 publitizedWorldPos;

    public void Awake() {
        publitizedWorldPos = RectTransformUtility.WorldToScreenPoint(null, GetComponent<RectTransform>().position);
    }
}
