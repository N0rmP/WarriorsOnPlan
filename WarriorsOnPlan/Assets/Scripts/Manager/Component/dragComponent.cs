using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum enumDrag { 
    none,
    bubbleStorage,
    bubbleInventory,
    thing
}

public class dragComponent : MonoBehaviour {
    private enumDrag curDragging_ = enumDrag.none;

    public enumDrag curDragging {
        get {
            return curDragging_;
        }
        set {
            if (curDragging_ == enumDrag.none) {
                curDragging_ = value;
            }
        }
    }

    private List<releasableObjectAbst> listReleasableObjects;

    public void Awake() {
        listReleasableObjects = new List<releasableObjectAbst>();
    }

    public bool relayRelease(System.Object[] parParameters) {
        bool tempIsWorkWell = false;

        // check if any releasableObjects is hovered, if so make it do its job
        foreach (releasableObjectAbst RO in listReleasableObjects) {
            if (RO.isActiveAndEnabled && RO.checkHovered()) {
                // if two or above releasableObjects are released spontaneously, tempIsWorkWell is true when any of them worked well... it's very rare case I think
                //Debug.Log("released : " + RO);
                tempIsWorkWell = tempIsWorkWell || RO.receiveRelease(parParameters);
            }
        }

        curDragging_ = enumDrag.none;

        return tempIsWorkWell;
    }

    #region listManagement
    public void addReleasableObject(releasableObjectAbst parRO) {
        listReleasableObjects.Add(parRO);
    }

    public void removeRleasableObject(releasableObjectAbst parRO) {
        listReleasableObjects.Remove(parRO);
    }

    public void clearListReleasableObjects() {
        listReleasableObjects.Clear();
    }
    #endregion listManagement
}
