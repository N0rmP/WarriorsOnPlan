using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class test : MonoBehaviour {
    public void Awake() {
        save(AssetPreview.GetAssetPreview(Resources.Load("Prefabs/tester")).EncodeToPNG());        
    }

    public void save(byte[] parByte) {
        System.IO.File.WriteAllBytes("Assets/Resources/Test/test.png", parByte);
    }

    public void sayAnything() {
        Debug.Log("!!! SHOUT LOUD !!!");
    }
}
