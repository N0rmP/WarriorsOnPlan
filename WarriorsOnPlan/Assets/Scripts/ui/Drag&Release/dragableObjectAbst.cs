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
    public void doAfterReleased(bool parIsAllDone) {
        if (parIsAllDone) {
            leave();
        }else{
            if (isReturnWhenReleased) {
                transform.SetParent(parentToReturn);
                thisRectTransform.localPosition = posReturn;
            } else {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        thisRectTransform.localPosition = Input.mousePosition - new Vector3(gameManager.GM.option.screenWidth / 2.0f, gameManager.GM.option.screenHeight / 2.0f, 0f);
    }

    public virtual void OnBeginDrag(PointerEventData eventData) {
        gameManager.GM.DC.curDragging = thisDrag;
        curPointerEventData = eventData;

        if (isReturnWhenReleased) {
            parentToReturn = transform.parent;
            posReturn = thisRectTransform.localPosition;
        }

        transform.SetParent(transformCanvasDragableWandering);

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
