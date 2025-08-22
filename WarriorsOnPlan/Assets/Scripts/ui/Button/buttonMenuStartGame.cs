using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMenuStartGame : MonoBehaviour {
    private uiActivatable uaCanvasModeSelection;

    public void Awake() {
        uaCanvasModeSelection = GameObject.Find("canvasModeSelection").GetComponent<uiActivatable>();
    }

    public void click() {
        uaCanvasModeSelection.activatePanel(new Vector3(0f, 0f, 0f));
    }
}
