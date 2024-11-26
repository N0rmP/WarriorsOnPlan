using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class test : MonoBehaviour {
    public GameObject Texts;

    public void foo() {
        string temp = "temp name ";
        int[] tteemp = new int[2] { Random.Range(1, 30), Random.Range(1, 30) };
        for (int i = 0; i < tteemp[0]; i++) {
            temp += "temp name ";
        }
        Texts.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp;
        temp = "temp info ";
        for (int i = 0; i < tteemp[1]; i++) {
            temp += "temp info ";
        }
        Texts.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = temp;
        Debug.Log(tteemp[0] + " , " + tteemp[1]);
        LayoutRebuilder.ForceRebuildLayoutImmediate(Texts.GetComponent<RectTransform>());
    }

    public void sayAnything() {
        Debug.Log("!!! SHOUT LOUD !!!");
    }
}
