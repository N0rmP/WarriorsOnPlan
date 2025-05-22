using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class releasableCurtainOutsideBI : releasableObjectAbst {
    public override void Start() {
        base.Start();
        gameObject.SetActive(false);
    }

    protected override bool doWhenReleased(object[] parParameters) {
        combatUIManager.CUM.CStatus.removeTool((Cases.caseBase)parParameters[0]);
        return true;
    }
}
