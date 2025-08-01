using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonResume : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.W, click);
    }

    public void click() {
        combatManager.CM.resumeREENACT();
    }
}
