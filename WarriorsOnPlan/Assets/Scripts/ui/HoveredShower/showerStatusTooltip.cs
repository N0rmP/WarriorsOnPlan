using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerStatusTooltip : showerText {
    [SerializeField]
    private int indexInSAS = 0;

    public new void Start() {
        base.Start();

        initText(
            gameManager.GM.DHouC.bookStatusTooltip.SwissArmyStringArray[indexInSAS * 2],
            gameManager.GM.DHouC.bookStatusTooltip.SwissArmyStringArray[indexInSAS * 2 + 1]
        );
    }
}