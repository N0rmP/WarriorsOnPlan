using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class dragableObjectAbst : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private static PointerEventData curPointerEventData = null;

    protected enumDrag thisDrag = enumDrag.none;

    protected RectTransform thisRectTransform;

    // protected bool isForPreparing = true;
    protected bool isReturnWhenReleased = true;
    private static Transform transformCanvasDragableWandering;
    private Transform parentToReturn;
    private Vector3 posReturn;

    public void Awake() {
        if (!TryGetComponent<RectTransform>(out thisRectTransform)) {
            thisRectTransform = gameObject.AddComponent<RectTransform>();
        }

        transformCanvasDragableWandering = GameObject.Find("canvasDraggableWandering").transform;
    }

    // parIsAllDone is true when dragableObject did its job completely and it's time for it to be gone, it's different from returning
    private void doAfterReleased(bool parIsAllDone) {
        if (parIsAllDone) {
            leave();
        }else{
            if (isReturnWhenReleased) {
                transform.SetParent(parentToReturn);
                transform.localScale = Vector3.one;
                transform.localPosition = posReturn;
            } else {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        thisRectTransform.localPosition = (Vector2)(Input.mousePosition) / gameManager.GM.canvasMain.transform.localScale - (gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta * 0.5f);
    }

    public virtual void OnBeginDrag(PointerEventData eventData) {
        gameManager.GM.DC.curDragging = thisDrag;
        curPointerEventData = eventData;

        if (isReturnWhenReleased) {
            parentToReturn = transform.parent;
            posReturn = thisRectTransform.localPosition;
        }

        transform.SetParent(transformCanvasDragableWandering);
        transform.localScale = Vector3.one;

        doWhenHoveringStart();
    }

    public virtual void OnEndDrag(PointerEventData eventData) {
        curPointerEventData = null;

        doAfterReleased(
            gameManager.GM.DC.relayRelease(thisDrag, getDragableParameters())
        );

        doWhenHoveringEnd();
    }

    // emergencyLeave is used to stop dragging forcefully
    public static void emergencyEndDrag() {
        if (curPointerEventData != null) {
            dragableObjectAbst tempDOA = curPointerEventData.pointerDrag.GetComponent<dragableObjectAbst>();
            curPointerEventData.dragging = false;
            curPointerEventData.pointerDrag = null;
            tempDOA.OnEndDrag(curPointerEventData);
        }
    }

    protected virtual void doWhenHoveringStart() { }
    protected virtual void doWhenHoveringEnd() { }
    protected abstract System.Object[] getDragableParameters();
    protected abstract void leave();
}
