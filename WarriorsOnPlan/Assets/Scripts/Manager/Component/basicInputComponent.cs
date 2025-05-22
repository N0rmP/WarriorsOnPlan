using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class basicInputComponent : MonoBehaviour{
    private Action delOnAnyKeyDown;
    private Dictionary<KeyCode, Action> dictKeyAction;

    public void Awake() {
        delOnAnyKeyDown = () => { };
        dictKeyAction = new Dictionary<KeyCode, Action>();
    }

    public void Update() {
        if (Input.anyKeyDown) {
            delOnAnyKeyDown();
            foreach (KeyCode kc in dictKeyAction.Keys) {
                if (Input.GetKeyDown(kc)) {
                    dictKeyAction[kc]();
                }
            }
        }
    }
}
