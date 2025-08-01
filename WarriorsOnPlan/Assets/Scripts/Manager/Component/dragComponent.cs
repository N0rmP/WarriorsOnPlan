using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum enumDrag { 
    anything = 0b1111,
    none = 0b000,
    bubbleStorage = 0b1,
    bubbleInventory = 0b10,
    thingOriginal = 0b100,
    thingActionOrder = 0b1000
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

        /* ★ 
        이론적으로 releasableObject를 dragComponent에 추가하다가 씬이 전환될 때 dragComponent를 초기화하고,
        새로운 씬에서 새로이 releasableObject를 추가하는 게 이상적이다.
        
        그러나 releasableObject는 자기자신이 생성될 때에만 dragComponent에 추가되기 때문에 씬이 전환됐다가 다시 전환되어 돌아오면 작동하지 않을 것이며,
        releasableObject는 제각기 생성/사용되는 타이밍이 다르므로 씬이 다시 전환된 이후 어떻게 다시 dragComponent에 추가시킬지가 난감하다.
        
        ISceneTransitioner를 사용해 자기자신이 사용될 씬이 활성화되면 추가할 수 있지만 둘 이상의 씬에서 사용될지도 모르는 releasableObject도 있어 완벽하지 않다.

        임의로 아래 주석처리된 dragComponent 정리정돈을 생략하고 개발해본 다음, 오버헤드가 클 경우 위 문제를 생각해보는 게 좋겠다.
        */
        //SceneManager.activeSceneChanged += (x, y) => clearListReleasableObjects();
    }

    public bool relayRelease(enumDrag parCurDragging, System.Object[] parParameters) {
        bool tempIsWorkWell = false;

        // check if any releasableObjects is hovered, if so make it do its job
        foreach (releasableObjectAbst RO in listReleasableObjects) {
            if (RO.isActiveAndEnabled && RO.checkHovered()) {
                // if two or above releasableObjects are released spontaneously, tempIsWorkWell is true when any of them worked well... it's very rare case I think
                // Debug.Log("released : " + RO + " / " + parCurDragging);
                tempIsWorkWell = RO.receiveRelease(parCurDragging, parParameters) || tempIsWorkWell;
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
        foreach (releasableObjectAbst roa in listReleasableObjects.ToArray()) {
            if (roa != null) {
                Destroy(roa);
            }
        }
        listReleasableObjects.Clear();
    }
    #endregion listManagement
}
