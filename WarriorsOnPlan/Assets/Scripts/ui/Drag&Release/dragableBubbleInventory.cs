using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;
using System;

public class dragableBubbleInventory : dragableBubbleAbst, IPointerClickHandler {
    public override void Awake() {
        base.Awake();
        thisDrag = enumDrag.bubbleInventory;
    }

    protected override void doWhenHoveringStart() {
        combatUIManager.CUM.closeCurtainOutsideBI();
    }

    protected override void doWhenHoveringEnd(){
        combatUIManager.CUM.openCurtainOutsideBI();
    }
    
    public Thing owner { get; private set; }

    protected override System.Object[] getParameters() {
        return new System.Object[1] { thisTool_ };
    }

    protected override void leave() {
        combatUIManager.CUM.CStatus.RI.removeBubble(this, true);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            combatUIManager.CUM.CStatus.RI.removeBubble(this, true);
        }
    }
}
