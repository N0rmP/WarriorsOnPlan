using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;
using System;

public class dragableBubbleStorage : dragableBubbleAbst {

    public new void Awake() {
        base.Awake();
        thisDrag = enumDrag.bubbleStorage;
    }

    protected override System.Object[] getDragableParameters() {
        return new System.Object[1] { thisTool_ };
    }

    protected override void leave() {
        combatManager.CM.CUM.TS.removeBubble(this);
    }    
}
