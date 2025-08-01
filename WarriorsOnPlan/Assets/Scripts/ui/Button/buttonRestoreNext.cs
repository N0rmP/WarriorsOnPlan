using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonRestoreNext : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.E, click);
    }

    public void click() {
        combatManager.CM.restoreNextAction();
    }
}
