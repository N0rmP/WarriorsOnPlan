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
        thisDrag = enumDrag.thing;
    }

    public void init(Thing parThing) {
        if (thisThing == null) {
            thisThing = parThing;
        }
    }

    protected override object[] getParameters() {
        return new System.Object[1] { thisThing };
    }

    // dragablePersonal doesn't leave, it only returns to its owner always
    protected override void leave() { }

    // dragablePersonal is only dragableObject of world-space-canvas, it needs several extra GUI process to work properly
    protected override void doWhenHoveringStart() {
        RectTransform tempRect = GetComponent<RectTransform>();
        tempRect.localRotation = Quaternion.Euler(0f, 0f, 0f);
        tempRect.anchorMin = new Vector2(0.5f, 0.5f);
        tempRect.anchorMax = new Vector2(0.5f, 0.5f);
        tempRect.sizeDelta = new Vector2(130f, 130f);

        GetComponent<Image>().sprite = thisThing.portrait;
        GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.8f);
    }

    protected override void doWhenHoveringEnd() {
        RectTransform tempRect = GetComponent<RectTransform>();
        tempRect.localRotation = Quaternion.Euler(0f, 0f, 0f);
        tempRect.localPosition = Vector3.zero;
        tempRect.anchorMin = new Vector2(0f, 0f);
        tempRect.anchorMax = new Vector2(1f, 1f);
        tempRect.offsetMin = new Vector2(0f, 0f);
        tempRect.offsetMax = new Vector2(0f, 0f);

        GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
    }
}
