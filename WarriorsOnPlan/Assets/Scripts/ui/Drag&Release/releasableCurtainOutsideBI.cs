using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class releasableCurtainBoxInner : releasableObjectAbst {
    public override void Start() {
        base.Start();
        targetEnumDrag = (int)enumDrag.bubbleInventory;
    }

    protected override bool doWhenReleased(enumDrag parCurDragging, object[] parParameters) {
        if (combatManager.CM.combatState != enumCombatState.preparing) {
            return false;
        }

        combatUIManager.CUM.CStatus.removeTool((Cases.caseBase)parParameters[0]);
        return true;
    }
}
