using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonResetInitial : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.T, askResetInitial);
    }

    public void askResetInitial() {
        // ★ 에휴... 문장 데이터로 가져와서 바꾸기
        gameManager.GM.PC.showPopupConfirm(
                "All State Returns to the Initial State.",
                () => combatManager.CM.BEPREPARED(true)
            );
    }
}
