using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

// you cannot use IDropHandler because it cannot detect dropping when currently dragged GameObject hides the mouse cursor
public abstract class releasableObjectAbst : MonoBehaviour {
    protected RectTransform thisRectTransform;

    public virtual void Start() {
        thisRectTransform = GetComponent<RectTransform>();
        gameManager.GM.DC.addReleasableObject(this);
    }

    public virtual bool checkHovered() {
        return thisRectTransform.checkHovered();
    }

    // �� �̰� dragComponent.curDragging�� this.enumDragRequired�� ���ؼ� true�� ������ �����ϰ� �ص� �� �� ������ �� ��������
    public bool receiveRelease(System.Object[] parParameters) {
        // try statement do check the types of parameters
        try {
            return doWhenReleased(parParameters);
        } catch (InvalidCastException e) {
            // if invalid arguement is passed from dragableObject to releasableObject, this try & catch statement will make it usless
            return false;
        } catch (Exception e) {
            Debug.Log("unexpected error on " + this + " ((" + e);
            return false;
        }
    }

    protected abstract bool doWhenReleased(System.Object[] parParameters);
}
