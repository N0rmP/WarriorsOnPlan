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

    public bool receiveRelease(System.Object[] parParameters) {
        // try statement do check the types of parameters
        try {
            return doWhenReleased(parParameters);
        } catch (InvalidCastException e) {
            String tempErrorMessage = "drag & drop error : ";
            foreach (System.Object obj in parParameters) {
                tempErrorMessage += obj.ToString() + ",";
            }
            tempErrorMessage += " are delivered to " + gameObject + " (" + e + ")";
            Debug.Log(tempErrorMessage);
            return false;
        } catch (Exception e) {
            Debug.Log("unexpected error on " + this + " " + e);
            return false;
        }
    }

    protected abstract bool doWhenReleased(System.Object[] parParameters);
}
