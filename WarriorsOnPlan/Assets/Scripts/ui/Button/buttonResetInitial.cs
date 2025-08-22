using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonResetInitial : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.T, askResetInitial);
    }

    public void askResetInitial() {
        gameManager.GM.PC.showPopupConfirm(
                gameManager.GM.DHouC.bookConfirmQuestion.strQuestionResetInitial,
                () => combatManager.CM.BEPREPARED(true)
            );
    }
}
