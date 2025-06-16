using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBePrepared : MonoBehaviour {
    public void Start() {
        gameManager.GM.BIC.addKeyActionPair(KeyCode.R, BePrepared);
    }

    public void BePrepared() {
        if (combatManager.CM.combatState < enumCombatState.combat || combatManager.CM.combatState > enumCombatState.reenact) {
            return;
        }

        combatManager.CM.BEPREPARED();
    }
}
