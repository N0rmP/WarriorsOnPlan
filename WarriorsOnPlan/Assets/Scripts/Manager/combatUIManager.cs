using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class combatUIManager : MonoBehaviour {
    public static combatUIManager CUM { get; private set; }

    public GameObject prefabBubble;

    public GameObject actionCounter { get; private set; }

    public toolStorage TS { get; private set; }
    public canvasStatus CStatus { get; private set; }
    public canvasStatistics CStatistics { get; private set; }
    public canvasCircuitSetter CCS { get; private set; }

    private GameObject curtainOutsideBI;

    public void Awake() {
        if (CUM == null) {
            CUM = this;
        } else {
            Destroy(gameObject);
        }

        prefabBubble = Resources.Load<GameObject>("Prefab/UI/bubbleTool");

        actionCounter = GameObject.Find("boxActionCounter");

        TS = GameObject.Find("scrollToolStorage").GetComponent<toolStorage>();
        CStatus = GameObject.Find("canvasStatus").GetComponent<canvasStatus>();
        CStatistics = GameObject.Find("canvasStatistics").GetComponent<canvasStatistics>();
        CCS = GameObject.Find("canvasCircuitSetter").GetComponent<canvasCircuitSetter>();

        curtainOutsideBI = GameObject.Find("curtainOutsideBI");
    }

    // if parIsStrict is false change numbers proceeds softly by ascending/descending gradually, if true just set the text once
    public void setActionCounter(int parValue, bool parIsStrict = false) {
        if (parIsStrict) {
            gameManager.GM.UC.addCount(actionCounter.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), parValue);
        } else {
            actionCounter.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = parValue.ToString();
        }
    }

    public void openCurtainOutsideBI() {
        curtainOutsideBI.SetActive(false);
    }

    public void closeCurtainOutsideBI() {
        curtainOutsideBI.SetActive(true);
    }

    #region test
    // ★ 이거 나중에 정식으로 만들어야 함
    public void testShowTurn() {
        TextMeshProUGUI tempTMPro = GameObject.Find("TEMP_TEXT_TURN").GetComponent<TextMeshProUGUI>();

        if (combatManager.CM.combatState != enumCombatState.reenact) {
            tempTMPro.text = "";
            return;
        }
        
        switch (combatManager.CM.sideTurn) {
            case enumSide.player:
                tempTMPro.text = "Player Turn";
                tempTMPro.color = Color.blue;
                break;
            case enumSide.enemy:
                tempTMPro.text = "Enemy Turn";
                tempTMPro.color = Color.red;
                break;
            default:
                tempTMPro.text = "Probably Neutral Turn";
                tempTMPro.color = Color.yellow;
                break;
        }
    }
    #endregion test
}
