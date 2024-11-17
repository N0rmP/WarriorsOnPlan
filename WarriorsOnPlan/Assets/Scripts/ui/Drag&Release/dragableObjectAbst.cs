using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class dragableObjectAbst : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private static GameObject canvasMain = null;

    protected enumDrag thisDrag = enumDrag.none;

    private RectTransform thisRectTransform;

    protected bool isForPreparing = true;
    protected bool isReturnWhenReleased = true;
    private Transform transformParent;
    private Vector3 posReturn;

    public virtual void Awake() {
        if (canvasMain == null) {
            canvasMain = GameObject.Find("CANVAS") ?? null;
        }

        if (!TryGetComponent<RectTransform>(out thisRectTransform)){
            Destroy(this);
        }
    }

    // parIsAllDone is true when dragableObject did its job completely and it's time for it to be gone, it's different from returning
    public void doAfterReleased(bool parIsAllDone) {
        if (parIsAllDone) {
            leave();
        }else{
            if (isReturnWhenReleased) {
                transform.SetParent(transformParent);
                //thisRectTransform.localPosition = posReturn;
            } else {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        thisRectTransform.localPosition = Input.mousePosition - new Vector3(optionAIO.screenWidth / 2.0f, optionAIO.screenHeight / 2.0f, 0f);
    }

    public virtual void OnBeginDrag(PointerEventData eventData) {
        gameManager.GM.DC.curDragging = thisDrag;

        if (isReturnWhenReleased) {
            transformParent = transform.parent;
            posReturn = thisRectTransform.localPosition;
        }

        transform.SetParent(canvasMain.transform);

        doWhenHoveringStart();
    }

    public virtual void OnEndDrag(PointerEventData eventData) {
        doAfterReleased(
            gameManager.GM.DC.relayRelease(
                getParameters()
                )
            );

        doWhenHoveringEnd();
    }

    protected virtual void doWhenHoveringStart() { }
    protected virtual void doWhenHoveringEnd() { }
    protected abstract System.Object[] getParameters();
    protected abstract void leave();
}
