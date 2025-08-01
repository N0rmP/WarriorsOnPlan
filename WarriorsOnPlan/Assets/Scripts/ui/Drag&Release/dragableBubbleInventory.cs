using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;
using System;

// ★ 드래그 도중 resetInitial 단축키로 때리고 다시 인벤토리에 놓으려고 하면 놓이긴 하는데 장비 증발함
public class dragableBubbleInventory : dragableBubbleAbst, IPointerClickHandler {
    public Thing owner { get; private set; }

    public new void Awake() {
        base.Awake();
        thisDrag = enumDrag.bubbleInventory;
    }

    protected override void doWhenHoveringStart() {
        base.doWhenHoveringStart();
        combatManager.CM.CUM.closeCurtainOutsideBI();
    }

    protected override void doWhenHoveringEnd(){
        base.doWhenHoveringEnd();
        combatManager.CM.CUM.openCurtainOutsideBI();
    }

    protected override System.Object[] getDragableParameters() {
        return new System.Object[1] { thisTool_ };
    }

    protected override void leave() {
        combatManager.CM.CUM.CStatus.removeBubble(this, true);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            combatManager.CM.CUM.CStatus.removeBubble(this, true);
        }
    }
}
