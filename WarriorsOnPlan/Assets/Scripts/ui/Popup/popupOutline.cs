using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class popupOutline : popupText {
    private TMP_FontAsset fontassetOutlineBlack;
    private TMP_FontAsset fontassetOutlineWhite;    

    public new void Awake() {
        base.Awake();

        fontassetOutlineBlack = Resources.Load<TMP_FontAsset>("Fonts/NEXON Lv2 Gothic Bold OutlinedBlack SDF");
        fontassetOutlineWhite = Resources.Load<TMP_FontAsset>("Fonts/NEXON Lv2 Gothic Bold OutlineWhite SDF");
    }

    // setPopupOutline changes font with more discrete outline going well with parTextColor
    public void setPopupOutline(Color parTextColor, Color parBackgroundColor, string parString = "", float parDuration = -1f) {
        thisText.font = (parTextColor.r * 0.2f + parTextColor.g * 0.7f + parTextColor.b * 0.1f < 128) ? fontassetOutlineWhite : fontassetOutlineBlack;
        setPopupText(parTextColor, parBackgroundColor, parString, parDuration);
    }

    protected override void returnThis() {
        gameManager.GM.PC.returnFloatingSingle(this);
    }
}
