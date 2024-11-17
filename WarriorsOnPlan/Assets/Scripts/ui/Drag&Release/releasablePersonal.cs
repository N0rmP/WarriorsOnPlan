using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// releasablePersonal won't be made on non-plrSide Things, they ain't controllable
public class releasablePersonal : releasableObjectAbst {
    private Thing thisThing = null;

    public override void Start() {
        base.Start();
    }

    public void Update() {
        thisThing.setCursorHovered(checkHovered());
    }

    public void init(Thing parThing) {
        if (thisThing == null) {
            thisThing = parThing;
        }
    }

    private bool grabTool(caseBase parTool) {
        // check parTool is tool, it's needed because tool checking is through enumCaseType not just Type
        if (parTool.caseType != enumCaseType.tool) {
            return false;
        }

        thisThing.addCase(parTool);
        return true;
    }

    private bool swapPosition(Thing parThing) {
        // Thing을 드래그 해서 드랍했을 때 thisThing과 parThing의 위치를 서로 변경하기 
        return false;
    }

    // releasablePersonal is in worldspace, it needs its own checkHovered method
    public override bool checkHovered() {
        return gameObject.checkHoveredWorld();
    }

    protected override bool doWhenReleased(System.Object[] parParameters) {
        if (!combatManager.CM.checkControllability(thisThing)) {
            return false;
        }

        switch (parParameters[0]) {
            case caseBase { caseType : enumCaseType.tool } tempTool:
                grabTool(tempTool);
                return true;
            case Thing tempThing:
                swapPosition(tempThing);
                return true;
            default:
                return false;
        }
    }
}
