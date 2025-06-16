using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class basicInputComponent : MonoBehaviour{
    public Action delOnAnyKeyDown;
    private Dictionary<KeyCode, Action> dictKeyAction;

    public void Awake() {
        delOnAnyKeyDown = null;
        dictKeyAction = new Dictionary<KeyCode, Action>();
    }

    public void Update() {
        if (Input.anyKeyDown) {
            if (delOnAnyKeyDown != null) {
                delOnAnyKeyDown();
            }
            
            foreach (KeyCode kc in dictKeyAction.Keys.ToArray()) {
                if (Input.GetKeyDown(kc)) {
                    if (dictKeyAction[kc] is not null) {
                        dictKeyAction[kc]();
                    }
                    break;
                }
            }
        }
    }

    public void addKeyActionPair(KeyCode parKeyCode, Action parAction, bool parIsReplace = false) {
        if (parIsReplace && dictKeyAction.ContainsKey(parKeyCode)) {
            Debug.Log("Action in KeyCode " + parKeyCode + " gonna be replaced, please check it if not intended");
            dictKeyAction[parKeyCode] = parAction;
            return;
        }

        if (dictKeyAction.ContainsKey(parKeyCode)) {
            dictKeyAction[parKeyCode] += parAction;
        } else {
            dictKeyAction.Add(parKeyCode, parAction);
        }
    }

    public void removeKeyAction(KeyCode parKeyCode, Action parAction) {
        if (!dictKeyAction.ContainsKey(parKeyCode)) {
            Debug.Log("basicInputComponent.removeKeyAction tried to remove action from null key, key was " + parKeyCode);
            return;
        }

        dictKeyAction[parKeyCode] -= parAction;
    }

    public void addAnyKeyAction(Action parAction, bool parIsReplace = false) {
        if (parIsReplace) {
            Debug.Log("AnyKeyAction gonna be replaced, please check it if not intended");
            delOnAnyKeyDown = parAction;
            return;
        }

        delOnAnyKeyDown += parAction;
    }

    public void clearAction() {
        delOnAnyKeyDown = null;
        dictKeyAction.Clear();
    }
}
