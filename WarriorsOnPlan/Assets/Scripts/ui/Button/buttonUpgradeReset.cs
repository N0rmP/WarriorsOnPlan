using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonUpgradeReset : MonoBehaviour {
    public void click() {
        mapManager.MM.MUC.CU.undoAllUpgrade();
        for (int i = 0; i < 3; i++) {
            mapManager.MM.MUC.CU.getBoxUpgradeTree(i).frontlineFirstUpgrade();
        }
    }
}
