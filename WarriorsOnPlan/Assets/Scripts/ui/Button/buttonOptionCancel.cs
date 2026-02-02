using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSimpleDeactivatePanel : MonoBehaviour {
    private uiActivatable uaTarget;

    public void Awake() {
        Transform tempTR = transform;
        while (uaTarget == null) {
            if (!tempTR.TryGetComponent<uiActivatable>(out uaTarget)) {
                if (tempTR.parent == null) {
                    Debug.Log(gameObject.name + ".buttonSimpleDeactivatePanel.Awake error : no uiActivatable found from parents");
                    break;
                } else {
                    tempTR = tempTR.parent;
                }
            }
        }
    }

    public void click() {
        uaTarget?.deactivatePanel();
    }
}
