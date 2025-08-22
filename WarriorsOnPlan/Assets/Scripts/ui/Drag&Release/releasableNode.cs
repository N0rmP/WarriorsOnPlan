using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class releasableNode : releasableObjectAbst {
    private node thisNode;

    public new void Start() {
        base.Start();
        targetEnumDrag = (int)enumDrag.thingOriginal;
    }

    public void Update() {
        if (checkHovered() && (gameManager.GM.DC.curDragging & enumDrag.thingOriginal) != 0) {
            thisNode.setColor(new Color(1f, 1f, 0f, 1f));
        } else {
            thisNode.autoColor();
        }
    }

    public void init(node parNode) {
        if (thisNode == null) {
            thisNode = parNode;
        }
    }

    public override bool checkHovered() {
        return gameObject.checkHoveredWorld();
    }

    protected override bool doWhenReleased(enumDrag parCurDragging, object[] parParameters) {
        Thing tempThingGuest = (Thing)parParameters[0];
        if (!combatManager.CM.checkControllability(tempThingGuest)) {
            return false;
        }

        Thing tempThingHost = thisNode.thingHere;
        tempThingGuest.curPosition.swapThing(thisNode, true);

        // arrage ActionOrder-Line, doWhenReleased should be assured to be called only when enumCombatState.preparing
        if (tempThingGuest is not null) {
            combatManager.CM.CUM.SAO.arrangeLineSingle(tempThingGuest);
        }
        if (tempThingHost is not null) {
            combatManager.CM.CUM.SAO.arrangeLineSingle(tempThingHost);
        }

        // canvasPersonal will be used constantly while preparing stop, it should not leave until combat starts
        return false;
    }
}
