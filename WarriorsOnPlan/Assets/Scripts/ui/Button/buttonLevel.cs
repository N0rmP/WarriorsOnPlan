using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// LEVEL IS BUTTON

public class buttonLevel : MonoBehaviour {
    private int thisLevelCode = 90101;
    private bool isBossLevel;

    public void enterLevel() {
        gameManager.GM.SceC.transitionSceneCombat(thisLevelCode, mapManager.MM.UC.arrUpgradeDone);
    }

    public void prepareButton(dataLevel parThisLevel, bool parIsClear) {
        thisLevelCode = parThisLevel.LevelCode;
        this.isBossLevel = parThisLevel.IsBossLevel;

        // set boss skull
        transform.GetChild(4).gameObject.SetActive(parThisLevel.IsBossLevel);

        updateButton(parIsClear);
    }

    public void updateButton(bool parIsClear) {
        // set text
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (thisLevelCode / 100 % 10) + "-" + (thisLevelCode % 10);

        // set total color
        if (parIsClear) {
            GetComponent<Image>().color = new Color(0.5f, 1f, 0.5f, 1f);
            transform.GetChild(1).GetComponent<Image>().color = new Color(0f, 0.75f, 0f, 1f);
        } else {
            GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f, 1f);
            transform.GetChild(1).GetComponent<Image>().color = new Color(0.75f, 0f, 0f, 1f);
        }

        // set stars
        transform.GetChild(2).gameObject.SetActive(parIsClear);
        transform.GetChild(3).gameObject.SetActive(parIsClear && isBossLevel);
    }
}
