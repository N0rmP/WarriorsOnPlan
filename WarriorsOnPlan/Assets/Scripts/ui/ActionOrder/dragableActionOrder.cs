using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class dragableActionOrder : dragableObjectAbst {
    // dragableActionOrder is basically child-object of releasableActionOrder, RAOoriginal means the parent-object of this dragableActionOrder
    private releasableActionOrder RAOoriginal;

    public new void Awake() {
        base.Awake();

        RAOoriginal = transform.parent.GetComponent<releasableActionOrder>();
        thisDrag = enumDrag.thingActionOrder;
    }

    protected override object[] getDragableParameters() {
        return new System.Object[2] { RAOoriginal.thisThing, RAOoriginal };
    }

    // dragablePersonal doesn't leave, it only returns to its releasableActionOrder always
    protected override void leave() {  }

    // dragablePersonal is only dragableObject of world-space-canvas, it needs several extra GUI process to work properly
    protected override void doWhenHoveringStart() {
        thisRectTransform.sizeDelta = new Vector2(130f, 130f);

        GetComponent<Image>().sprite = RAOoriginal.thisThing.portrait;
        GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.8f);
    }

    protected override void doWhenHoveringEnd() {
        thisRectTransform.sizeDelta = new Vector2(100f, 100f);

        GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
    }
}
