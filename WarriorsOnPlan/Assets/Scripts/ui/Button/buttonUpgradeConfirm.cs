using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonUpgradeConfirm : MonoBehaviour {
    public void click() {
        mapManager.MM.MUC.CU.GetComponent<uiActivatable>().deactivatePanel();
    }
}
