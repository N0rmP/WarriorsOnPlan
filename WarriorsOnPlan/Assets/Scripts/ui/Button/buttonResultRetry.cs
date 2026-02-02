using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BCR is BoxCombatResult
public class buttonResultRetry : MonoBehaviour {
    public void click() {
        combatManager.CM.CUM.BCR.deactivate();
        gameManager.GM.IC.dismissTemporayInputContinaer();

        combatManager.CM.BEPREPARED();
    }
}