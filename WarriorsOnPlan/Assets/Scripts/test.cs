using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
//using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using Cases;
using UnityEngine.Localization.Settings;
using System.Text;

// [CustomEditor(typeof(Button))]
public class test : MonoBehaviour {
    public int test_marker;

    public void Start() {
        Debug.Log("it is test.Start()");
        PriorityQueue<int> pq = new PriorityQueue<int>();
        /*
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 20; i++) {
            sb.Clear();

            if (i < 10) {
                pq.Enqueue(UnityEngine.Random.Range(0, 99));
            } else {
                sb.Append("Dequeued : ");
                sb.Append(pq.Dequeue());
            }

            sb.Append("\npq : ");
            foreach (int j in pq) {
                sb.Append(j);
                sb.Append(", ");
            }
            Debug.Log(sb.ToString());
            
        }
        */
        for (int i = 0; i< 10; i++) {
            pq.Enqueue(UnityEngine.Random.Range(0, 99));
        }
        pq.testPriorityQueue();
    }

    public void Update() { }

    public virtual void testShout() {
        Debug.Log(gameObject.name + " : test shout");
    }

    public virtual void testShout(string parStr) {
        Debug.Log(gameObject.name + " : test shout / " + parStr);
    }
}