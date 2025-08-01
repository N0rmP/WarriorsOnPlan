using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerStatusTooltip : showerText {
    private static dataArbitraryStringArray? dataStatusTooltip = null;

    [SerializeField]
    private int indexInSAS = 0;

    public new void Awake() {
        base.Awake();

        if (dataStatusTooltip == null) {
            dataStatusTooltip = gameManager.GM.FC.importResourcesJson<dataArbitraryStringArray>("JustText/statusTooltip");
        }

        initText(
            dataStatusTooltip?.SwissArmyStringArray[indexInSAS * 2],
            dataStatusTooltip?.SwissArmyStringArray[indexInSAS * 2 + 1]
        );
    }
}
