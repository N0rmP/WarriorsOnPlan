using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMenuModeSelect : MonoBehaviour{
    public enumMapType thisEnumMapType;

    public void click() {
        gameManager.GM.setCurMapType(thisEnumMapType);
        gameManager.GM.SceC.transitionSceneMap();
        mapManager.MM.prepareMap();
    }
}
