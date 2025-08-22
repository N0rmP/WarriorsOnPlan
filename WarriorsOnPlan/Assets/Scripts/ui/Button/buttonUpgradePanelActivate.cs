using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonUpgradePanelActivate : MonoBehaviour {
    public void click() {
        mapManager.MM.UC.restoreUpgrade(gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType));
        mapManager.MM.MUC.CU.GetComponent<uiActivatable>().activatePanel(new Vector3(0f, 0f, 0f));
    }
}
