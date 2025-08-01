using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonRestorePrev : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.Q, click);
    }

    public void click() {
        combatManager.CM.restorePreviousAction();
    }
}
