using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBePrepared : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.R, BePrepared);
    }

    public void BePrepared() {
        if (combatManager.CM.combatState < enumCombatState.combat) {
            return;
        }
        
        combatManager.CM.BEPREPARED();
    }
}
