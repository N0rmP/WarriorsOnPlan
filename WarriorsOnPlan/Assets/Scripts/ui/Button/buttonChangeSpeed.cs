using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class buttonChangeSpeed : MonoBehaviour {
    public void Awake() {
        gameManager.GM.IC.addKeyActionPair("SceneCombat", KeyCode.Tab, click);
    }

    public void click() {
        combatManager.CM.changeSpeed();

        TextMeshProUGUI tempText;
        if (transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out tempText)) {
            tempText.text = "X" + combatManager.CM.combatSpeed;
        }
    }
}
