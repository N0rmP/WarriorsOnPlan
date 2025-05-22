using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imgEffect : showerCase {
    protected override void init() {
        base.init();
        setCaseTypeShown((int)Cases.enumCaseType.effect);
    }

    protected override void doWhenSetCase() {
        GetComponent<imgRoundRectangle>().setImg(thisCase.caseImage);
    }
}
