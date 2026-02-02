using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonResultExit : MonoBehaviour {
    public void click() {
        combatManager.CM.systemDestroyLevel();
        combatManager.CM.CUM.BCR.deactivate();
        gameManager.GM.IC.dismissTemporayInputContinaer();

        gameManager.GM.SceC.transitionSceneMap();
    }
}
