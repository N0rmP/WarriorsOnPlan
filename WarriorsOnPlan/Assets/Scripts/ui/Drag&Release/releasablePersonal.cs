using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Cases;

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

    private void swapPosition(Thing parThing) {
        node tempThisThingPosition = thisThing.curPosition;
        node tempParThingPosition = parThing.curPosition;

        tempParThingPosition.expelThing();
        thisThing.curPosition.sendThing(tempParThingPosition, true);

        tempThisThingPosition.placeThing(parThing);
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
            case caseBase { caseType: enumCaseType.tool } tempTool:
                return grabTool(tempTool);
            case Thing tempThing:
                swapPosition(tempThing);
                return false;
            default:
                return false;
        }        
    }
}
