using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class buttonSimple : MonoBehaviour {
    public void activateBoxCircuitConcrete(int parCircuitTypeBeingChosen) {
        combatUIManager.CUM.CCS.activateBoxCircuitConcrete(parCircuitTypeBeingChosen);
    }

    public void confirmCircuitSetting() {
        combatUIManager.CUM.CStatus.confirmCircuitSetting();
    }    

    public void restorePrev() {
        combatManager.CM.restorePreviousAction();
    }

    public void resumeCombat() {
        combatManager.CM.resumeREENACT();
    }

    public void changeSpeed() {
        combatManager.CM.changeSpeed();

        TextMeshProUGUI tempText;
        if (transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out tempText)) {
            tempText.text = "X" + combatManager.CM.combatSpeed;
        }
    }
}
