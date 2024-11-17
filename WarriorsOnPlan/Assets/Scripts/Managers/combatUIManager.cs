using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatUIManager : MonoBehaviour {
    public static combatUIManager CUM { get; private set; }

    public GameObject prefabBubble { get; private set; }

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

        prefabBubble = Resources.Load<GameObject>("Prefabs/UI/bubbleTool");

        TS = GameObject.Find("scrollToolStorage").GetComponent<toolStorage>();
        CStatus = GameObject.Find("canvasStatus").GetComponent<canvasStatus>();
        CStatistics = GameObject.Find("canvasStatistics").GetComponent<canvasStatistics>();
        CCS = GameObject.Find("canvasCircuitSetter").GetComponent<canvasCircuitSetter>();

        curtainOutsideBI = GameObject.Find("curtainOutsideBI");
    }

    public void openCurtainOutsideBI() {
        curtainOutsideBI.SetActive(false);
    }

    public void closeCurtainOutsideBI() {
        curtainOutsideBI.SetActive(true);
    }
}
