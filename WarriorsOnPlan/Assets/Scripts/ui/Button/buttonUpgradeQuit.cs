using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonUpgradeQuit : MonoBehaviour {
    [SerializeField]
    private bool isSave;

    public void click() {
        if (isSave) {
            mapManager.MM.UC.confirmUpgrade();
            mapManager.MM.UC.saveUpgrade();
        }
        mapManager.MM.MUC.CU.GetComponent<uiActivatable>().deactivatePanel();
    }
}
