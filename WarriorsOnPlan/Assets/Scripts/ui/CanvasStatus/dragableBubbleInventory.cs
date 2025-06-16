using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;
using System;

public class dragableBubbleInventory : dragableBubbleAbst, IPointerClickHandler {
    public Thing owner { get; private set; }

    public new void Awake() {
        base.Awake();
        thisDrag = enumDrag.bubbleInventory;
    }

    protected override void doWhenHoveringStart() {
        base.doWhenHoveringStart();
        combatUIManager.CUM.closeCurtainOutsideBI();
    }

    protected override void doWhenHoveringEnd(){
        base.doWhenHoveringEnd();
        combatUIManager.CUM.openCurtainOutsideBI();
    }

    protected override System.Object[] getDragableParameters() {
        return new System.Object[1] { thisTool_ };
    }

    protected override void leave() {
        combatUIManager.CUM.CStatus.removeBubble(this, true);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            combatUIManager.CUM.CStatus.removeBubble(this, true);
        }
    }
}
