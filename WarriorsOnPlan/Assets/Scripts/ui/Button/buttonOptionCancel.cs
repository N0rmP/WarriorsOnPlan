using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSimpleDeactivatePanel : MonoBehaviour {
    public uiActivatable uaTarget;

    public void click() {
        uaTarget.deactivatePanel();
    }
}
