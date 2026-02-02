using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class dragablePersonal : dragableObjectAbst {
    private Thing thisThing = null;

    public new void Awake() {
        base.Awake();
        thisDrag = enumDrag.thingOriginal;
    }

    public void init(Thing parThing) {
        if (thisThing == null) {
            thisThing = parThing;
        }
    }

    protected override object[] getDragableParameters() {
        return new System.Object[1] { thisThing };
    }

    // dragablePersonal doesn't leave, it only returns to its owner always
    protected override void leave() { }

    // dragablePersonal is only dragableObject of world-space-canvas, it needs several extra GUI process to work properly
    protected override void doWhenHoveringStart() {
        thisRectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        thisRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        thisRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        thisRectTransform.sizeDelta = new Vector2(130f, 130f);

        GetComponent<Image>().sprite = thisThing.portrait;
        GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.8f);

        if (thisThing.thisPlacabler != null) {
            combatManager.CM.GC.setPlacableNode(thisThing.thisPlacabler);
        }       
    }

    protected override void doWhenHoveringEnd() {
        thisRectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        thisRectTransform.localPosition = Vector3.zero;
        thisRectTransform.anchorMin = new Vector2(0f, 0f);
        thisRectTransform.anchorMax = new Vector2(1f, 1f);
        thisRectTransform.offsetMin = new Vector2(0f, 0f);
        thisRectTransform.offsetMax = new Vector2(0f, 0f);

        GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

        if (thisThing.thisPlacabler != null) {
            combatManager.CM.GC.setPlacableNode(combatManager.CM.PC.curPlacabler);
        }
    }
}
