using Processes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonCombatStart : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.Space, startCombat);
    }

    public void startCombat() {
        switch (combatManager.CM.combatState) {
            case enumCombatState.preparing:
                combatManager.CM.startCombat();
                break;
            case enumCombatState.reenactHalted:     // while testing I mis-pressed space as resume-button so many times
                combatManager.CM.resumeREENACT();
                break;
            default:
                break;
        }
    }
}
