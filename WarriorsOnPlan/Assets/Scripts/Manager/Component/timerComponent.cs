using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class timerComponent : MonoBehaviour
{
    private Dictionary<Action, float> dictDelegate;

    void Awake() {
        dictDelegate = new Dictionary<Action, float>();
    }

    void Update()
    {
        float tempDeltaTime = Time.deltaTime;
        foreach (Action del in dictDelegate.Keys.ToArray()) {
            dictDelegate[del] -= tempDeltaTime;
            if (dictDelegate[del] < 0f) {
                del();
                dictDelegate.Remove(del);
            }
        }
    }

    #region utility
    public void addDelegate(Action parDelegate, float parDelayTimer) {
        dictDelegate.Add(parDelegate, parDelayTimer);
    }

    public void clearDelegate() {
        dictDelegate.Clear();
    }
    #endregion utility
}
