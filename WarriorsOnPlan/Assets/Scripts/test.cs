using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.ToString() + " _ anchored  : " + GetComponent<RectTransform>().anchoredPosition);
        Debug.Log(gameObject.ToString() + " _ local     : " + GetComponent<RectTransform>().localPosition);
    }

    public void sayAnything() {
        Debug.Log("!!! SHOUT LOUD !!!");
    }
}
