using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class releasableNode : releasableObjectAbst {
    private node thisNode;

    public void Update() {
        if (gameManager.GM.DC.curDragging == enumDrag.thing && gameObject.checkHoveredWorld()) {
            thisNode.setColor(new Color(1f, 1f, 0f, 1f));
        } else {
            thisNode.returnColor();
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

    protected override bool doWhenReleased(object[] parParameters) {
        ((Thing)parParameters[0]).curPosition.sendThing(thisNode, true);
        // canvasPersonal will be used constantly while preparing stop, it should not leave until combat starts
        return false;
    }
}
